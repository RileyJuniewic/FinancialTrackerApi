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
        public async Task<IActionResult> OpenAccount(OpenAccountRequest request) =>
            Ok(await _savingsAccountService.OpenSavingsAccount(request));
        
        [HttpPost]
        [Route("CloseAccount")]
        public async Task<IActionResult> CloseAccount(CloseAccountRequest request) =>
            Ok(await _savingsAccountService.CloseSavingsAccount(request));

        [HttpPost]
        [Route("NewTransaction")]
        public async Task<IActionResult> NewTransaction(TransactionRequest request) =>
            Ok(await _savingsAccountService.AddTransaction(request));
        
        [HttpPost]
        [Route("Transfer")]
        public async Task<IActionResult> TransferToAccount(TransferRequest request) =>
            Ok(await _savingsAccountService.TransferToAccount(request));

        [HttpGet]
        public async Task<IActionResult> GetAccount(string savingsAccountId) =>
            Ok(await _savingsAccountService.GetSavingsAccount(savingsAccountId));
        
        [HttpGet]
        [Route("Accounts")]
        public async Task<IActionResult> GetAccounts() =>
            Ok(await _savingsAccountService.GetSavingsAccounts());
        
        [HttpGet]
        [Route("Transactions")]
        public async Task<IActionResult> GetAccountTransactions(string savingsAccountId) =>
            Ok(await _savingsAccountService.GetAccountTransactions(savingsAccountId));
    }
}
