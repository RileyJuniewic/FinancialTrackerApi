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
        Task<SavingsAccount> GetSavingsAccountAsync(string accountId);
        Task<IEnumerable<SavingsAccount>> GetSavingsAccountsAsync();
        Task<SavingsAccount> OpenSavingsAccountAsync(OpenAccountRequest request);
        Task<SavingsAccount> CloseSavingsAccountAsync(CloseAccountRequest request);
        Task<TransactionResponse> AddTransactionAsync(TransactionRequest request);
        Task<TransferResponse> TransferToAccountAsync(TransferRequest request);
        Task<IEnumerable<Transaction>> GetAccountTransactionsAsync(string accountId, int offset, int rowLimit);
        Task<SavingsAccount> ChangeAccountNameAsync(AccountNameChangeRequest request);
        Task<Transaction> EditTransactionAsync(EditTransactionRequest request);
        Task<TransactionTypesSumFromRangeResponse> GetTransactionSumDataAsync(TransactionTypesSumFromRangeRequest request);
    }

    public class SavingsAccountService(
        ISqlDataAccess sqlDataAccess,
        IHttpContextHelperService httpContext,
        IUserService userService)
        : ISavingsAccountService
    {
        public async Task<SavingsAccount> GetSavingsAccountAsync(string accountId)
        {
            var userId = httpContext.GetClaimUserId().Value;
            var savingsAccount = (await sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>("GetSavingsAccount",
                    new { @id = accountId, @userid = userId }, commandType: CommandType.StoredProcedure))
                .FirstOrDefault();

            return savingsAccount ?? throw new Exception("The provided account id is invalid.");
        }

        public async Task<IEnumerable<SavingsAccount>> GetSavingsAccountsAsync()
        {
            var userId = httpContext.GetClaimUserId();
            var savingsAccounts = (await sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>
            ("GetSavingsAccounts", new { @id = userId.Value }, commandType: CommandType.StoredProcedure)).ToList();

            return savingsAccounts.Any() ? savingsAccounts : throw new Exception("Cannot find savings accounts");
        }

        public async Task<SavingsAccount> OpenSavingsAccountAsync(OpenAccountRequest request)
        {
            var userId = httpContext.GetClaimUserId();
            var account = SavingsAccount.CreateNew(userId.Value, request.AccountName, request.InitialBalance);
            await sqlDataAccess.GetConnection().ExecuteAsync("OpenSavingsAccount", account, commandType: CommandType.StoredProcedure);
            return account;
        }

        public async Task<SavingsAccount> CloseSavingsAccountAsync(CloseAccountRequest request)
        {
            await userService.VerifyCredentialsAsync(request.LoginRequest);
            
            var account = await GetSavingsAccountAsync(request.AccountId);
            if (account.Balance != 0) throw new Exception("Savings account balance must be 0.00");

            var result = await sqlDataAccess.GetConnection().ExecuteAsync("DeleteSavingsAccount",
                new { @accountId = account.Id, @userId = account.UserId },
                commandType: CommandType.StoredProcedure);

            return result < 0 ? throw new Exception("Savings account balance must be 0.00") : account;
        }

        public async Task<TransactionResponse> AddTransactionAsync(TransactionRequest request)
        {
            var account = await GetSavingsAccountAsync(request.AccountId);
            
            var transaction = Transaction.CreateNewTransaction(request.AccountId, request.Type, request.Description,
                request.Amount, account, request.Date);

            await sqlDataAccess.GetConnection()
                .ExecuteAsync("AddTransaction", transaction, commandType: CommandType.StoredProcedure);

            return new TransactionResponse(transaction, account);
        }

        public async Task<TransferResponse> TransferToAccountAsync(TransferRequest request)
        {
            var transferInAccount = await GetSavingsAccountAsync(request.ReceiverAccountId);
            var transferOutAccount = await GetSavingsAccountAsync(request.AccountId);
            
            if (transferInAccount.UserId != transferOutAccount.UserId)
                throw new Exception("Accounts used in transfer must be the same.");

            var transferInTransaction = Transaction.CreateNewTransaction(transferInAccount.Id,
                TransactionType.TransferIn, request.Description, request.TransferAmount, transferInAccount, request.Date);
            
            var transferOutTransaction = Transaction.CreateNewTransaction(transferOutAccount.Id,
                TransactionType.TransferOut, request.Description, request.TransferAmount, transferOutAccount, request.Date);
            
            var transfer = ConvertToTransfer(transferInTransaction, transferOutTransaction, request.Date);

            var result = await sqlDataAccess.GetConnection()
                .ExecuteAsync("TransferToAccount", transfer, commandType: CommandType.StoredProcedure);
            
            return result < 0 ? throw new Exception("Cannot complete transfer") :
                new TransferResponse(transferOutTransaction, transferInTransaction);
        }

        public async Task<IEnumerable<Transaction>> GetAccountTransactionsAsync(string accountId, int offset, int rowLimit)
        {
            var userId = httpContext.GetClaimUserId().Value;
            return await sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransactionsFromAccount", new { @accountId, @userId, @offset, @rowLimit },
                    commandType: CommandType.StoredProcedure);
        }

        public async Task<SavingsAccount> ChangeAccountNameAsync(AccountNameChangeRequest request)
        {
            var account = await GetSavingsAccountAsync(request.AccountId);
            account.ChangeName(request.Name);
            await sqlDataAccess.GetConnection().ExecuteAsync("UpdateAccountName",
                new { request.AccountId, account.UserId, request.Name }, commandType: CommandType.StoredProcedure);
            return account;
        }

        public async Task<Transaction> EditTransactionAsync(EditTransactionRequest request)
        {
            var account = await GetSavingsAccountAsync(request.AccountId);
            var initialTransaction = await GetTransactionAsync(account.Id, request.TransactionId);

            if (initialTransaction.TransactionType == "Transfer")
                throw new Exception("Transactions of type transfer are not editable.");
            
            var type = Transaction.StringToTransactionType(initialTransaction.TransactionType);
            var newTransaction = Transaction.CreateExistingTransaction(initialTransaction.Id, initialTransaction.SavingsAccountId, 
                type, request.Description, initialTransaction.Amount, initialTransaction.NewBalance, request.Date);
            
            await sqlDataAccess.GetConnection()
                .ExecuteAsync("EditTransaction",
                    new
                    {
                        initialTransaction.Id, initialTransaction.SavingsAccountId, request.Description, request.Date
                    }, commandType: CommandType.StoredProcedure);

            return newTransaction;
        }

        public async Task<TransactionTypesSumFromRangeResponse> GetTransactionSumDataAsync(TransactionTypesSumFromRangeRequest request)
        {
            var account = await GetSavingsAccountAsync(request.SavingsAccountId);
            var transactions = (await sqlDataAccess.GetConnection().QueryAsync<Transaction>("GetTransactionsInRange",
                new { @savingsAccountId = account.Id, request.StartDate, request.EndDate },
                commandType: CommandType.StoredProcedure)).ToList();

            decimal depositSum = 0, withdrawalSum = 0, transferInSum = 0, transferOutSum = 0;
            
            foreach (var transaction in transactions)
            {
                switch (transaction.TransactionType)
                {
                    case "Deposit": { depositSum += transaction.Amount; }
                        break;
                    case "Withdrawal": { withdrawalSum += transaction.Amount; }
                        break;
                    case "Transfer In": { transferInSum += transaction.Amount; }
                        break;
                    case "Transfer Out": { transferOutSum += transaction.Amount; }
                        break;
                }
            }
            
            return new TransactionTypesSumFromRangeResponse(depositSum,withdrawalSum,transferInSum,transferOutSum);
        }

        private static Transfer ConvertToTransfer(Transaction transactionIn,
            Transaction transactionOut, DateTime date, string? transferId = null)
        {
            transferId ??= Guid.NewGuid().ToString();

            if (transactionIn.Amount != transactionOut.Amount)
                throw new Exception("Transfer failed.");
            
            return new Transfer(transferId, transactionOut.Id, transactionOut.SavingsAccountId,
                transactionOut.NewBalance, transactionOut.TransactionType, transactionIn.Id,
                transactionIn.SavingsAccountId, transactionIn.NewBalance, transactionIn.TransactionType,
                transactionIn.Amount, date, transactionOut.Description);
        }

        private async Task<Transaction> GetTransactionAsync(string accountId, string transactionId)
        {
             var response = (await sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransaction", new { @accountId, @transactionId }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
             if (response == null)
                 throw new Exception("The provided transaction id is invalid.");
             return response;
        }
    }
}
