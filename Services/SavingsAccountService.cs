using Dapper;
using FinancialTracker.Common.Contracts.SavingsAccount;
using FinancialTracker.Common.Errors;
using FinancialTracker.Models;
using FinancialTracker.Persistance;
using FinancialTracker.Services.Common;
using System.Data;
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
        Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId);
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

            return savingsAccount ?? throw Errors.SavingsAccountError.CannotFindAccount;
        }


        public async Task<IEnumerable<SavingsAccount>> GetSavingsAccounts()
        {
            var userId = _httpContext.GetClaimUserId();
            var savingsAccounts = (await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>
            ("GetSavingsAccounts", new { @id = userId.Value }, commandType: CommandType.StoredProcedure)).ToList();

            return savingsAccounts.Any() ? savingsAccounts : throw Errors.SavingsAccountError.CannotFindAccount;
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
            if (account.Balance != "0.00") throw Errors.SavingsAccountError.BalanceMustBeZero;

            var result = await _sqlDataAccess.GetConnection().ExecuteAsync("DeleteSavingsAccount",
                new { @accountId = account.Id, @userId = account.UserId },
                commandType: CommandType.StoredProcedure);

            return result < 0 ? throw Errors.SavingsAccountError.BalanceMustBeZero : account;
        }

        public async Task<TransactionResponse> AddTransaction(TransactionRequest request)
        {
            var account = await GetSavingsAccount(request.SavingsAccountId);
            var currencyAmount = SavingsAccount.StringToCurrencyString(request.Amount);
            
            var transaction = Transaction.CreateNewTransaction(account.UserId, request.SavingsAccountId,
                request.Type, request.Description, currencyAmount, account);

            await _sqlDataAccess.GetConnection()
                .ExecuteAsync("AddTransaction", transaction, commandType: CommandType.StoredProcedure);

            return new TransactionResponse(transaction, account);
        }

        public async Task<TransferResponse> TransferToAccount(TransferRequest request)
        {
            await _userService.VerifyCredentialsAsync(request.LoginRequest);
            
            var receiverAccount = await GetSavingsAccount(request.ReceiverAccountId);
            var senderAccount = await GetSavingsAccount(request.AccountId);
            
            var currencyTransferAmount = SavingsAccount.StringToCurrencyString(request.TransferAmount);
            senderAccount.Transfer(receiverAccount, currencyTransferAmount);

            var transfer = ConvertToTransfer(senderAccount, receiverAccount, DateTime.UtcNow, request.Description,
                currencyTransferAmount);
            
            var result = await _sqlDataAccess.GetConnection()
                .ExecuteAsync("TransferToAccount", transfer, commandType: CommandType.StoredProcedure);
            
            return result < 0 ? throw Errors.SavingsAccountError.SqlErrorCannotCompleteTransfer :
                new TransferResponse(senderAccount.Id, senderAccount.Name, senderAccount.Balance, receiverAccount.Id);
        }

        public async Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId)
        {
            var userId = _httpContext.GetClaimUserId().Value;
            return await _sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransactionsFromAccount", new { @accountId, @userId },
                    commandType: CommandType.StoredProcedure);
        }

        private static Transfer ConvertToTransfer(SavingsAccount sender, SavingsAccount receiver, DateTime date,
            string description, string transferTotal, string? transferId = null, string? senderTransId = null,
            string? receiverTransId = null)
        {
            transferId ??= Guid.NewGuid().ToString();
            senderTransId ??= Guid.NewGuid().ToString();
            receiverTransId ??= Guid.NewGuid().ToString();
            
            return new Transfer(transferId, senderTransId, sender.Id, sender.UserId, sender.Balance, receiverTransId,
                receiver.Id, receiver.UserId, receiver.Balance, transferTotal, date, description);
        }
    }
}
