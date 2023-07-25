using Dapper;
using FinancialTracker.Models;
using FinancialTracker.Persistance;
using FinancialTracker.Services.Common;
using System.Data;
using FinancialTracker.Common.Contracts.SavingsAccount.Request;
using FinancialTracker.Common.Contracts.SavingsAccount.Response;
using FinancialTracker.Models.Enums;
using FinancialTracker.Services.Records;

namespace FinancialTracker.Services
{
    public interface ISavingsAccountService
    {
        Task<SavingsAccount> GetSavingsAccount(string accountId);
        Task<IEnumerable<SavingsAccount>> GetSavingsAccounts();
        Task<SavingsAccount> OpenSavingsAccount(OpenAccountRequest request);
        Task<SavingsAccount> CloseSavingsAccount(CloseAccountRequest request);
        Task<TransactionResponse> AddTransaction(TransactionRequest request);
        Task<TransferResponse> TransferToAccount(TransferRequest request);
        Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId, int offset, int rowLimit);
        Task<SavingsAccount> ChangeAccountName(AccountNameChangeRequest request);
        Task<Transaction> EditTransaction(EditTransactionRequest request);
        Task<TransactionTypesSumFromRangeResponse> GetTransactionSumsFromRange(TransactionTypesSumFromRangeRequest request);
    }

    public class SavingsAccountService : ISavingsAccountService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly IHttpContextHelperService _httpContext;
        private readonly IUserService _userService;

        public SavingsAccountService(ISqlDataAccess sqlDataAccess, IHttpContextHelperService httpContext, IUserService userService)
        {
            _sqlDataAccess = sqlDataAccess;
            _httpContext = httpContext;
            _userService = userService;
        }

        public async Task<SavingsAccount> GetSavingsAccount(string accountId)
        {
            var userId = _httpContext.GetClaimUserId().Value;
            var savingsAccount = (await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>("GetSavingsAccount",
                    new { @id = accountId, @userid = userId }, commandType: CommandType.StoredProcedure))
                .FirstOrDefault();

            return savingsAccount ?? throw new Exception("The provided account id is invalid.");
        }

        public async Task<IEnumerable<SavingsAccount>> GetSavingsAccounts()
        {
            var userId = _httpContext.GetClaimUserId();
            var savingsAccounts = (await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>
            ("GetSavingsAccounts", new { @id = userId.Value }, commandType: CommandType.StoredProcedure)).ToList();

            return savingsAccounts.Any() ? savingsAccounts : throw new Exception("Cannot find savings accounts");
        }

        public async Task<SavingsAccount> OpenSavingsAccount(OpenAccountRequest request)
        {
            var userId = _httpContext.GetClaimUserId();
            var account = SavingsAccount.CreateNew(userId.Value, request.AccountName, request.InitialBalance);
            await _sqlDataAccess.GetConnection().ExecuteAsync("OpenSavingsAccount", account, commandType: CommandType.StoredProcedure);
            return account;
        }

        public async Task<SavingsAccount> CloseSavingsAccount(CloseAccountRequest request)
        {
            await _userService.VerifyCredentialsAsync(request.LoginRequest);
            
            var account = await GetSavingsAccount(request.AccountId);
            if (account.Balance != "0.00") throw new Exception("Savings account balance must be 0.00");

            var result = await _sqlDataAccess.GetConnection().ExecuteAsync("DeleteSavingsAccount",
                new { @accountId = account.Id, @userId = account.UserId },
                commandType: CommandType.StoredProcedure);

            return result < 0 ? throw new Exception("Savings account balance must be 0.00") : account;
        }

        public async Task<TransactionResponse> AddTransaction(TransactionRequest request)
        {
            var account = await GetSavingsAccount(request.AccountId);
            
            var transaction = Transaction.CreateNewTransaction(request.AccountId, request.Type, request.Description,
                request.Amount, account);

            await _sqlDataAccess.GetConnection()
                .ExecuteAsync("AddTransaction", transaction, commandType: CommandType.StoredProcedure);

            return new TransactionResponse(transaction, account);
        }

        public async Task<TransferResponse> TransferToAccount(TransferRequest request)
        {
            var transferInAccount = await GetSavingsAccount(request.ReceiverAccountId);
            var transferOutAccount = await GetSavingsAccount(request.AccountId);
            
            var userId = transferInAccount.UserId;
            if (transferInAccount.UserId != transferOutAccount.UserId)
                throw new Exception("Accounts used in transfer must be the same.");

            var transferInTransaction = Transaction.CreateNewTransaction(transferInAccount.Id,
                TransactionType.TransferIn, request.Description, request.TransferAmount, transferInAccount);
            
            var transferOutTransaction = Transaction.CreateNewTransaction(transferOutAccount.Id,
                TransactionType.TransferOut, request.Description, request.TransferAmount, transferOutAccount);
            
            var transfer = ConvertToTransfer(userId, transferInTransaction, transferOutTransaction, DateTime.UtcNow);

            var result = await _sqlDataAccess.GetConnection()
                .ExecuteAsync("TransferToAccount", transfer, commandType: CommandType.StoredProcedure);
            
            return result < 0 ? throw new Exception("Cannot complete transfer") :
                new TransferResponse(transferOutTransaction, transferInTransaction);
        }

        public async Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId, int offset, int rowLimit)
        {
            var userId = _httpContext.GetClaimUserId().Value;
            return await _sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransactionsFromAccount", new { @accountId, @userId, @offset, @rowLimit },
                    commandType: CommandType.StoredProcedure);
        }

        public async Task<SavingsAccount> ChangeAccountName(AccountNameChangeRequest request)
        {
            var account = await GetSavingsAccount(request.AccountId);
            account.ChangeName(request.Name);
            await _sqlDataAccess.GetConnection().ExecuteAsync("UpdateAccountName",
                new { request.AccountId, account.UserId, request.Name }, commandType: CommandType.StoredProcedure);
            return account;
        }

        public async Task<Transaction> EditTransaction(EditTransactionRequest request)
        {
            var account = await GetSavingsAccount(request.AccountId);
            var initialTransaction = await GetTransaction(account.Id, request.TransactionId);

            if (initialTransaction.TransactionType == "Transfer")
                throw new Exception("Transactions of type transfer are not editable.");
            
            var type = Transaction.StringToTransactionType(initialTransaction.TransactionType);
            var newTransaction = Transaction.CreateExistingTransaction(initialTransaction.Id, initialTransaction.SavingsAccountId, 
                type, request.Description, initialTransaction.Amount, initialTransaction.NewBalance, request.Date);
            
            await _sqlDataAccess.GetConnection()
                .ExecuteAsync("EditTransaction",
                    new
                    {
                        initialTransaction.Id, initialTransaction.SavingsAccountId, request.Description, request.Date
                    }, commandType: CommandType.StoredProcedure);

            return newTransaction;
        }

        public async Task<TransactionTypesSumFromRangeResponse> GetTransactionSumsFromRange(TransactionTypesSumFromRangeRequest request)
        {
            var account = await GetSavingsAccount(request.SavingsAccountId);
            var transactions = (await _sqlDataAccess.GetConnection().QueryAsync<Transaction>("GetTransactionsInRange",
                new { @savingsAccountId = account.Id, request.StartDate, request.EndDate },
                commandType: CommandType.StoredProcedure)).ToList();

            float depositSum = 0, withdrawalSum = 0, transferInSum = 0, transferOutSum = 0;
            
            foreach (var transaction in transactions)
            {
                var floatAmount = SavingsAccount.StringToFloat(transaction.Amount);
                switch (transaction.TransactionType)
                {
                    case "Deposit": { depositSum += floatAmount; }
                        break;
                    case "Withdrawal": { withdrawalSum += floatAmount; }
                        break;
                    case "Transfer In": { transferInSum += floatAmount; }
                        break;
                    case "Transfer Out": { transferOutSum += floatAmount; }
                        break;
                }
            }
            
            return new TransactionTypesSumFromRangeResponse(SavingsAccount.FloatToCurrencyString(depositSum),
                SavingsAccount.FloatToCurrencyString(withdrawalSum),
                SavingsAccount.FloatToCurrencyString(transferInSum),
                SavingsAccount.FloatToCurrencyString(transferOutSum));
        }

        private static Transfer ConvertToTransfer(string userId, Transaction transactionIn,
            Transaction transactionOut, DateTime date, string? transferId = null)
        {
            transferId ??= Guid.NewGuid().ToString();

            if (transactionIn.Amount != transactionOut.Amount)
                throw new Exception("Transfer failed.");
            
            return new Transfer(transferId, userId, transactionOut.Id, transactionOut.SavingsAccountId,
                transactionOut.NewBalance, transactionOut.TransactionType, transactionIn.Id,
                transactionIn.SavingsAccountId, transactionIn.NewBalance, transactionIn.TransactionType,
                transactionIn.Amount, date, transactionOut.Description);
        }

        private async Task<Transaction> GetTransaction(string accountId, string transactionId)
        {
             var response = (await _sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransaction", new { @accountId, @transactionId }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
             if (response == null)
                 throw new Exception("The provided transaction id is invalid.");
             return response;
        }
    }
}
