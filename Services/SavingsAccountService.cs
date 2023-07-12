using Dapper;
using FinancialTracker.Models;
using FinancialTracker.Persistance;
using FinancialTracker.Services.Common;
using System.Data;
using FinancialTracker.Common.Contracts.SavingsAccount.Request;
using FinancialTracker.Common.Contracts.SavingsAccount.Response;
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
        Task<int> ChangeAccountName(AccountNameChangeRequest request);
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

            return savingsAccount ?? throw new Exception("Cannot find savings account");
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
            var currencyAmount = SavingsAccount.StringToCurrencyString(request.Amount);
            
            var transaction = Transaction.CreateNewTransaction(account.UserId, request.AccountId,
                request.Type, request.Description, currencyAmount, account);

            await _sqlDataAccess.GetConnection()
                .ExecuteAsync("AddTransaction", transaction, commandType: CommandType.StoredProcedure);

            return new TransactionResponse(transaction, account);
        }

        public async Task<TransferResponse> TransferToAccount(TransferRequest request)
        {
            var receiverAccount = await GetSavingsAccount(request.ReceiverAccountId);
            var senderAccount = await GetSavingsAccount(request.AccountId);
            
            var currencyTransferAmount = SavingsAccount.StringToCurrencyString(request.TransferAmount);
            senderAccount.Transfer(receiverAccount, currencyTransferAmount);

            var transfer = ConvertToTransfer(senderAccount, receiverAccount, DateTime.UtcNow, request.Description,
                currencyTransferAmount);
            
            var result = await _sqlDataAccess.GetConnection()
                .ExecuteAsync("TransferToAccount", transfer, commandType: CommandType.StoredProcedure);
            
            return result < 0 ? throw new Exception("Cannot complete transfer") :
                new TransferResponse(senderAccount.Id, senderAccount.Name, senderAccount.Balance, receiverAccount.Id);
        }

        public async Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId)
        {
            var userId = _httpContext.GetClaimUserId().Value;
            return await _sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransactionsFromAccount", new { @accountId, @userId },
                    commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ChangeAccountName(AccountNameChangeRequest request)
        {
            var userId = _httpContext.GetClaimUserId();            
            return await _sqlDataAccess.GetConnection().ExecuteAsync("UpdateAccountName",
                new { @request.AccountId, @userId = userId.Value, @request.Name }, commandType: CommandType.StoredProcedure);
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
