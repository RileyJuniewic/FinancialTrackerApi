using Dapper;
using FinancialTracker.Common.Contracts.SavingsAccount;
using FinancialTracker.Common.Errors;
using FinancialTracker.Models;
using FinancialTracker.Persistance;
using FinancialTracker.Services.Common;
using System.Data;

namespace FinancialTracker.Services
{
    public interface ISavingsAccountService
    {
        Task<SavingsAccount> GetSavingsAccount(string accountId);
        Task<IEnumerable<SavingsAccount>> GetSavingsAccounts();
        Task<SavingsAccount> OpenSavingsAccount(string accountName);
        Task<SavingsAccount> CloseSavingsAccount(CloseAccountRequest request);
        Task<TransactionResponse> AddTransaction(TransactionRequest request);
        Task<TransferResponse> TransferToAccount(TransferRequest request);
        Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId);
    }

    public class SavingsAccountService : ISavingsAccountService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly IHttpContextHelperService _httpHelperService;
        private readonly IUserService _userService;

        public SavingsAccountService(ISqlDataAccess sqlDataAccess, IHttpContextHelperService httpHelperService, IUserService userService)
        {
            _sqlDataAccess = sqlDataAccess;
            _httpHelperService = httpHelperService;
            _userService = userService;
        }

        public async Task<SavingsAccount> GetSavingsAccount(string accountId)
        {
            var userId = _httpHelperService.GetClaimUserId().Value;
            var savingsAccount = (await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>("GetSavingsAccount",
                    new { @id = accountId, @userid = userId }, commandType: CommandType.StoredProcedure))
                .FirstOrDefault();

            return savingsAccount ?? throw Errors.SavingsAccountError.CannotFindAccount;
        }


        public async Task<IEnumerable<SavingsAccount>> GetSavingsAccounts()
        {
            var userId = _httpHelperService.GetClaimUserId();
            var savingsAccounts = (await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>("GetSavingsAccounts",
                new { @id = userId.Value }, commandType: CommandType.StoredProcedure));

            return savingsAccounts.Any() ? savingsAccounts : throw Errors.SavingsAccountError.CannotFindAccount;
        }

        public async Task<SavingsAccount> OpenSavingsAccount(string accountName)
        {
            var userId = _httpHelperService.GetClaimUserId();
            var account = SavingsAccount.CreateNew(userId.Value, accountName);
            await _sqlDataAccess.GetConnection().ExecuteAsync("OpenSavingsAccount", account, commandType: CommandType.StoredProcedure);
            return account;
        }

        public async Task<SavingsAccount> CloseSavingsAccount(CloseAccountRequest request)
        {
            {
                var senderUser = await _userService.VerifyLogin(request.LoginRequest);
                var userId = _httpHelperService.GetClaimUserId();

                if (senderUser.Id != userId.Value)
                    throw Errors.SavingsAccountError.CannotVerifyLoginCredentials;
            }
            
            var account = await GetSavingsAccount(request.AccountId);
            if (account.Balance != "0.00") throw Errors.SavingsAccountError.BalanceMustBeZero;

            var result = await _sqlDataAccess.GetConnection().ExecuteAsync("DeleteSavingsAccount",
                new { @accountId = account.Id, @userId = account.UserId },
                commandType: CommandType.StoredProcedure);

            return result < 0 ? throw Errors.SavingsAccountError.BalanceMustBeZero : account;
        }

        public async Task<TransactionResponse> AddTransaction(TransactionRequest request)
        {
            var userId = _httpHelperService.GetClaimUserId();
            var account = await GetSavingsAccount(request.SavingsAccountId);
            var currencyAmount = SavingsAccount.StringToCurrencyString(request.Amount);

            var transaction = Transaction.CreateNewTransaction(userId.Value, request.SavingsAccountId,
                request.Type, request.Description, currencyAmount);

            var result = transaction.TransactionType switch
            {
                "Withdrawal" => account.Withdraw(transaction.Amount),
                "Deposit" => account.Deposit(transaction.Amount),
                _ => throw Errors.TransactionError.InvalidTransactionType
            };

            await _sqlDataAccess.GetConnection()
                .ExecuteAsync("AddTransaction",
                    new
                    {
                        @id = transaction.Id, @savingsAccountId = transaction.SavingsAccountId,
                        @transactionType = transaction.TransactionType, @description = transaction.Description,
                        @amount = transaction.Amount, @date = transaction.Date, @balance = result
                    }, commandType: CommandType.StoredProcedure);

            return new TransactionResponse(transaction, account);
        }

        public async Task<TransferResponse> TransferToAccount(TransferRequest request)
        {
            {
                var senderUser = await _userService.VerifyLogin(request.LoginRequest);
                var userId = _httpHelperService.GetClaimUserId();

                if (senderUser.Id != userId.Value)
                    throw Errors.SavingsAccountError.CannotVerifyLoginCredentials;
            }

            var receiverAccount = await GetSavingsAccount(request.ReceiverAccountId);
            var senderAccount = await GetSavingsAccount(request.AccountId);
            
            var currencyTransferAmount = SavingsAccount.StringToCurrencyString(request.TransferAmount);
            senderAccount.Transfer(receiverAccount, currencyTransferAmount);

            var result = await _sqlDataAccess.GetConnection()
                .ExecuteAsync("TransferToAccount",
                    new
                    {
                        @transferId = Guid.NewGuid().ToString(), @senderTransactionId = Transaction.CreateId(),
                        @senderAccountId = senderAccount.Id, @senderUserId = senderAccount.UserId,
                        @senderNewBalance = senderAccount.Balance, @receiverTransactionId = Transaction.CreateId(),
                        @receiverAccountId = receiverAccount.Id, @receiverUserId = receiverAccount.UserId, 
                        @receiverNewBalance = receiverAccount.Balance, @transferTotal = currencyTransferAmount,
                        @dateTime = DateTime.UtcNow, @description = request.Description
                    }, commandType: CommandType.StoredProcedure);
            
            return result < 0 ? throw Errors.SavingsAccountError.SqlErrorCannotCompleteTransfer :
                new TransferResponse(senderAccount.Id, senderAccount.Name, senderAccount.Balance, receiverAccount.Id);
        }

        public async Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId)
        {
            var userId = _httpHelperService.GetClaimUserId().Value;
            return await _sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransactionsFromAccount", new { @accountId, @userId },
                    commandType: CommandType.StoredProcedure);
        }
    }
}
