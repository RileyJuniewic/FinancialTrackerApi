namespace FinancialTracker.Services.Records;

public record Transfer(
    string TransferId,
    string SenderTransactionId,
    string SenderAccountId,
    string SenderUserid,
    string SenderNewBalance,
    string ReceiverTransactionId,
    string ReceiverAccountId,
    string ReceiverUserId,
    string ReceiverNewBalance,
    string TransferTotal,
    DateTime DateTime,
    string Description
    );
