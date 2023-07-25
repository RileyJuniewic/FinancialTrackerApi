namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record TransactionTypesSumFromRangeRequest(
        
    string SavingsAccountId,
    
    DateTime StartDate,
    
    DateTime EndDate
    
    );