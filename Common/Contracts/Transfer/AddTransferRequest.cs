﻿namespace FinancialTracker.Common.Contracts.Transfer;

public record AddTransferRequest(
    Guid AccountId
    ) : AccountBase(AccountId);