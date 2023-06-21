using FinancialTracker.Common.Contracts.SavingsAccount;
using FinancialTracker.Controllers.Common;
using FinancialTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers
{
    [Route("[controller]")]
    public class SavingsAccountController : ApiController
    {
        private readonly ISavingsAccountService _savingsAccountService;

        public SavingsAccountController(ISavingsAccountService savingsAccountService)
        {
            _savingsAccountService = savingsAccountService;
        }

        [HttpPost]
        [Route("OpenAccount")]
        public async Task<IActionResult> OpenAccount([FromBody] string accountName) =>
            (await _savingsAccountService.OpenSavingsAccount(accountName)).Match(Ok, Problem);
        
        [HttpPost]
        [Route("CloseAccount")]
        public async Task<IActionResult> CloseAccount(CloseAccountRequest request) =>
            (await _savingsAccountService.CloseSavingsAccount(request)).Match(Ok, Problem);

        [HttpPost]
        [Route("NewTransaction")]
        public async Task<IActionResult> NewTransaction(TransactionRequest request) =>
            (await _savingsAccountService.AddTransaction(request)).Match(Ok, Problem);
        
        [HttpPost]
        [Route("Transfer")]
        public async Task<IActionResult> TransferToAccount(TransferRequest request) =>
            (await _savingsAccountService.TransferToAccount(request)).Match(Ok, Problem);

        [HttpGet]
        public async Task<IActionResult> GetAccount(string savingsAccountId) =>
            (await _savingsAccountService.GetSavingsAccount(savingsAccountId)).Match(Ok, Problem);
        
        [HttpGet]
        [Route("Accounts")]
        public async Task<IActionResult> GetAccounts() =>
            (await _savingsAccountService.GetSavingsAccounts()).Match(Ok, Problem);
        
        [HttpGet]
        [Route("Transactions")]
        public async Task<IActionResult> GetAccountTransactions(string savingsAccountId) =>
            (await _savingsAccountService.GetAccountTransactions(savingsAccountId)).Match(Ok, Problem);
    }
}
