namespace FinancialTracker.Models;

public class Transfer
{
    public Transaction Sender { get; private set; }
    public Transaction Receiver { get; private set; }
    public Guid TransferId { get; private set; }

    protected Transfer(Transaction sender, Transaction receiver, Guid transferId)
    {
        Sender = sender;
        Receiver = receiver;
        TransferId = transferId;
    }
}