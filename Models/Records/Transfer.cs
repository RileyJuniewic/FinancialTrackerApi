namespace FinancialTracker.Services.Records;

public record Transfer(
    string TransferId,
    
    string TransferOutTransactionId,
    string TransferOutAccountId,
    decimal TransferOutNewBalance,
    string TransferOutType,
    
    string TransferInTransactionId,
    string TransferInAccountId,
    decimal TransferInNewBalance,
    string TransferInType,
    decimal TransferTotal,
    
    DateTime DateTime,
    string Description
    );
