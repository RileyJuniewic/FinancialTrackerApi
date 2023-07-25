namespace FinancialTracker.Services.Records;

public record Transfer(
    string TransferId,
    string UserId,
    
    string TransferOutTransactionId,
    string TransferOutAccountId,
    string TransferOutNewBalance,
    string TransferOutType,
    
    string TransferInTransactionId,
    string TransferInAccountId,
    string TransferInNewBalance,
    string TransferInType,
    string TransferTotal,
    
    DateTime DateTime,
    string Description
    );
