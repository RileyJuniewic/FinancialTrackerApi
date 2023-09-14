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
        private readonly IAccountService _accountService;

        public SavingsAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
/*
        [HttpPost]
        [Route("OpenAccount")]
        public async Task<IActionResult> OpenAccount(OpenAccountRequest request) =>
            Ok(await _accountService.OpenSavingsAccount(request));
        
        [HttpPost]
        [Route("CloseAccount")]
        public async Task<IActionResult> CloseAccount(CloseAccountRequest request) =>
            Ok(await _accountService.CloseSavingsAccount(request));

        [HttpPost]
        [Route("NewTransaction")]
        public async Task<IActionResult> NewTransaction(AddTransactionRequest request) =>
            Ok(await _accountService.AddTransaction(request));
        
        [HttpPost]
        [Route("Transfer")]
        public async Task<IActionResult> TransferToAccount(TransferRequest request) =>
            Ok(await _accountService.TransferToAccount(request));
        
        [HttpPost]
        [Route("DeleteTransaction")]
        public async Task<IActionResult> DeleteTransaction(DeleteTransactionRequest request) =>
            Ok(await _accountService.DeleteTransaction(request));

        [HttpGet]
        public async Task<IActionResult> GetAccount(string accountId) =>
            Ok(await _accountService.GetSavingsAccount(accountId));
        
        [HttpGet]
        [Route("Accounts")]
        public async Task<IActionResult> GetAccounts() =>
            Ok(await _accountService.GetSavingsAccounts());
        
        [HttpGet]
        [Route("Transactions")]
        public async Task<IActionResult> GetAccountTransactions(string accountId, int dbOffset, int dbRowLimit) =>
            Ok(await _accountService.GetAccountTransactions(accountId, dbOffset, dbRowLimit));

        [HttpGet]
        [Route("TransactionSumsFromRange")]
        public async Task<IActionResult>
            GetAccountTransactionSumsFromRange([FromQuery] TransactionTypesSumFromRangeRequest request) =>
            Ok(await _accountService.GetTransactionSumsFromRange(request));

        [HttpPatch]
        [Route("ChangeName")]
        public async Task<IActionResult> ChangeAccountName(AccountNameChangeRequest request) =>
            Ok(await _accountService.ChangeAccountName(request));

        [HttpPut]
        [Route("EditTransaction")]
        public async Task<IActionResult> EditTransaction(EditTransactionRequest request) =>
            Ok(await _accountService.EditTransaction(request));
            
            */
    }
}
