using FinancialTracker.Common.Contracts.Transfer;
using FinancialTracker.Models;

namespace FinancialTracker.Services;

public interface ITransferService
{
    Task<Transfer> AddTransfer(AddTransferRequest request);
    Task<Transfer> EditTransfer(EditTransferRequest request);
    Task<Transfer> DeleteTransfer(DeleteTransferRequest request);
}

public class TransferService : ITransferService
{
    public async Task<Transfer> AddTransfer(AddTransferRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Transfer> EditTransfer(EditTransferRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Transfer> DeleteTransfer(DeleteTransferRequest request)
    {
        throw new NotImplementedException();
    }
}