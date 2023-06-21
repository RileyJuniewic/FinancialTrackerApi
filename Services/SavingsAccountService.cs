using Dapper;
using ErrorOr;
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
        Task<ErrorOr<SavingsAccount>> GetSavingsAccount(string accountId);
        Task<ErrorOr<IEnumerable<SavingsAccount>>> GetSavingsAccounts();
        Task<ErrorOr<SavingsAccount>> OpenSavingsAccount(string accountName);
        Task<ErrorOr<SavingsAccount>> CloseSavingsAccount(CloseAccountRequest request);
        Task<ErrorOr<TransactionResponse>> AddTransaction(TransactionRequest request);
        Task<ErrorOr<TransferResponse>> TransferToAccount(TransferRequest request);
        Task<ErrorOr<IEnumerable<Transaction>>> GetAccountTransactions(string accountId);
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

        public async Task<ErrorOr<SavingsAccount>> GetSavingsAccount(string accountId)
        {
            if (_httpHelperService.GetClaimUserId() is var userId && userId.IsError)
            {
                return userId.Errors;
            }
            
            var result = (await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>("GetSavingsAccount",
                    new { @id = accountId, @userid = userId.Value.Value }, commandType: CommandType.StoredProcedure))
                .FirstOrDefault();

            return (result != null ? result : Errors.SavingsAccountError.CannotFindAccount);
        }

        public async Task<ErrorOr<IEnumerable<SavingsAccount>>> GetSavingsAccounts()
        {
            if (_httpHelperService.GetClaimUserId() is var userId && userId.IsError)
            {
                return userId.Errors;
            }

            var response = await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>("GetSavingsAccounts",
                new { @id = userId.Value.Value }, commandType: CommandType.StoredProcedure);

            return response.Any() ? ErrorOrFactory.From(response) : Errors.SavingsAccountError.CannotFindAccount;
        }

        public async Task<ErrorOr<SavingsAccount>> OpenSavingsAccount(string accountName)
        {
            if (_httpHelperService.GetClaimUserId() is var userId && userId.IsError)
            {
                return userId.Errors;
            }

            var account = SavingsAccount.CreateNew(userId.Value.Value, accountName);
            await _sqlDataAccess.GetConnection().ExecuteAsync("OpenSavingsAccount", account, commandType: CommandType.StoredProcedure);
            return account;
        }

        public async Task<ErrorOr<SavingsAccount>> CloseSavingsAccount(CloseAccountRequest request)
        {
            {
                var senderUser = await _userService.VerifyLogin(request.LoginRequest);
                if (senderUser.IsError) return Errors.SavingsAccountError.CannotVerifyLoginCredentials;

                var userId = _httpHelperService.GetClaimUserId();
                if (userId.IsError) return userId.Errors;

                if (senderUser.Value.Id != userId.Value.Value) 
                {return Errors.SavingsAccountError.CannotVerifyLoginCredentials;}
            }
            
            var account = await GetSavingsAccountDetails(request.AccountId);
            if (account.IsError) return account.Errors;

            if (account.Value.Balance != "0.00") return Errors.SavingsAccountError.BalanceMustBeZero;

            var result = await _sqlDataAccess.GetConnection().ExecuteAsync("DeleteSavingsAccount",
                new { @accountId = account.Value.Id, @userId = account.Value.UserId },
                commandType: CommandType.StoredProcedure);

            return result < 0 ? Errors.SavingsAccountError.BalanceMustBeZero : account;
        }

        public async Task<ErrorOr<TransactionResponse>> AddTransaction(TransactionRequest request)
        {
            var userId = _httpHelperService.GetClaimUserId();
            if (userId.IsError) return userId.Errors;

            var account = await GetSavingsAccount(request.SavingsAccountId);
            if (account.IsError) return account.Errors;

            var currencyAmount = SavingsAccount.StringToCurrencyString(request.Amount);
            if (currencyAmount.IsError) return currencyAmount.Errors;

            var transactionResult = Transaction.CreateNewTransaction(userId.Value.Value, request.SavingsAccountId,
                request.Type, request.Description, currencyAmount.Value);
            if (transactionResult.IsError) return transactionResult.Errors;

            var transaction = transactionResult.Value;
            var result = transaction.TransactionType switch
            {
                "Withdrawal" => account.Value.Withdraw(transaction.Amount),
                "Deposit" => account.Value.Deposit(transaction.Amount),
                _ => Errors.TransactionError.InvalidTransactionType
            };

            if (result.IsError) return result.Errors;

            await _sqlDataAccess.GetConnection()
                .ExecuteAsync("AddTransaction",
                    new
                    {
                        @id = transaction.Id,
                        @savingsAccountId = transaction.SavingsAccountId,
                        @transactionType = transaction.TransactionType, @description = transaction.Description,
                        @amount = transaction.Amount, @date = transaction.Date, @balance = result.Value
                    }, commandType: CommandType.StoredProcedure);

            return new TransactionResponse(transaction, account.Value);
        }

        public async Task<ErrorOr<TransferResponse>> TransferToAccount(TransferRequest request)
        {
            {
                var senderUser = await _userService.VerifyLogin(request.LoginRequest);
                if (senderUser.IsError) return Errors.SavingsAccountError.CannotVerifyLoginCredentials;

                var userId = _httpHelperService.GetClaimUserId();
                if (userId.IsError) return userId.Errors;

                if (senderUser.Value.Id != userId.Value.Value) 
                {return Errors.SavingsAccountError.CannotVerifyLoginCredentials;}
            }

            var receiverAccount = await GetSavingsAccountDetails(request.ReceiverAccountId);
            if (receiverAccount.IsError) return receiverAccount.Errors;

            var senderAccount = await GetSavingsAccountDetails(request.AccountId);
            if (senderAccount.IsError) return senderAccount.Errors;
            
            var currencyTransferAmount = SavingsAccount.StringToCurrencyString(request.TransferAmount);
            if (currencyTransferAmount.IsError) return currencyTransferAmount.Errors;

            var transfer = senderAccount.Value.Transfer(receiverAccount.Value, currencyTransferAmount.Value);
            if (transfer.IsError) return transfer.Errors;

            var result = await _sqlDataAccess.GetConnection()
                .ExecuteAsync("TransferToAccount",
                    new
                    {
                        @transferId = Guid.NewGuid().ToString(), @senderTransactionId = Transaction.CreateId(),
                        @senderAccountId = senderAccount.Value.Id, @senderUserId = senderAccount.Value.UserId,
                        @senderNewBalance = senderAccount.Value.Balance, @receiverTransactionId = Transaction.CreateId(),
                        @receiverAccountId = receiverAccount.Value.Id, @receiverUserId = receiverAccount.Value.UserId, 
                        @receiverNewBalance = receiverAccount.Value.Balance, @transferTotal = currencyTransferAmount.Value,
                        @dateTime = DateTime.UtcNow, @description = request.Description
                    }, commandType: CommandType.StoredProcedure);
            
            return result < 0 ? Errors.SavingsAccountError.SqlErrorCannotCompleteTransfer :
                new TransferResponse(senderAccount.Value.Id, senderAccount.Value.Name, 
                senderAccount.Value.Balance, receiverAccount.Value.Id);
        }

        public async Task<ErrorOr<IEnumerable<Transaction>>> GetAccountTransactions(string accountId)
        {
            if (_httpHelperService.GetClaimUserId() is var userId && userId.IsError) 
            { return userId.Errors; }

            var response = await _sqlDataAccess.GetConnection()
                .QueryAsync<Transaction>("GetTransactionsFromAccount",
                    new { @accountId },
                    commandType: CommandType.StoredProcedure);
            
            //! Validate account belongs to user
            
            return response.Any() ? ErrorOrFactory.From(response) : Errors.SavingsAccountError.CannotFindAccount;
        }

        private async Task<ErrorOr<SavingsAccount>> GetSavingsAccountDetails(string accountId)
        {
            var result = (await _sqlDataAccess.GetConnection().QueryAsync<SavingsAccount>("GetAccountDetails",
                new { accountId }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            return result is not null ? result : Errors.SavingsAccountError.CannotFindAccount;
        }
    }
}
