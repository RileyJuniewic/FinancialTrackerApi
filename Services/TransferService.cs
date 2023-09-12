using FinancialTracker.Common.Contracts.SavingsAccount.Request;
using FinancialTracker.Common.Contracts.Transaction;
using FinancialTracker.Common.Contracts.Transfer;
using FinancialTracker.Models;

namespace FinancialTracker.Services;

public interface ITransferService
{
    Task<Transfer> AddTransfer(TransferRequest request);
    Task<Transfer> DeleteTransfer(DeleteTransferRequest request);
    Task<Transfer> EditTransfer(EditTransferRequest request);
}

public class TransferService
{
    
}