using FinancialTracker.Common.Contracts.SavingsAccount.Request;
using FinancialTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SavingsAccountController : ControllerBase
    {
        private readonly ISavingsAccountService _savingsAccountService;

        public SavingsAccountController(ISavingsAccountService savingsAccountService)
        {
            _savingsAccountService = savingsAccountService;
        }

        [HttpPost]
        [Route("OpenAccount")]
        public async Task<IActionResult> OpenAccount(OpenAccountRequest request) =>
            Ok(await _savingsAccountService.OpenSavingsAccountAsync(request));
        
        [HttpPost]
        [Route("CloseAccount")]
        public async Task<IActionResult> CloseAccount(CloseAccountRequest request) =>
            Ok(await _savingsAccountService.CloseSavingsAccountAsync(request));

        [HttpPost]
        [Route("NewTransaction")]
        public async Task<IActionResult> NewTransaction(TransactionRequest request) =>
            Ok(await _savingsAccountService.AddTransactionAsync(request));
        
        [HttpPost]
        [Route("Transfer")]
        public async Task<IActionResult> TransferToAccount(TransferRequest request) =>
            Ok(await _savingsAccountService.TransferToAccountAsync(request));

        [HttpGet]
        public async Task<IActionResult> GetAccount(string accountId) =>
            Ok(await _savingsAccountService.GetSavingsAccountAsync(accountId));
        
        [HttpGet]
        [Route("Accounts")]
        public async Task<IActionResult> GetAccounts() =>
            Ok(await _savingsAccountService.GetSavingsAccountsAsync());
        
        [HttpGet]
        [Route("Transactions")]
        public async Task<IActionResult> GetAccountTransactions(string accountId, int dbOffset, int dbRowLimit) =>
            Ok(await _savingsAccountService.GetAccountTransactionsAsync(accountId, dbOffset, dbRowLimit));

        [HttpGet]
        [Route("TransactionSums")]
        public async Task<IActionResult>
            GetAccountTransactionSumsFromRange([FromQuery] TransactionTypesSumFromRangeRequest request) =>
            Ok(await _savingsAccountService.GetTransactionSumDataAsync(request));

        [HttpPatch]
        [Route("ChangeName")]
        public async Task<IActionResult> ChangeAccountName(AccountNameChangeRequest request) =>
            Ok(await _savingsAccountService.ChangeAccountNameAsync(request));

        [HttpPut]
        [Route("EditTransaction")]
        public async Task<IActionResult> EditTransaction(EditTransactionRequest request) =>
            Ok(await _savingsAccountService.EditTransactionAsync(request));
    }
}
