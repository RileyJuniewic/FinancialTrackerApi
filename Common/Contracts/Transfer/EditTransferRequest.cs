﻿namespace FinancialTracker.Common.Contracts.Transfer;

public record EditTransferRequest(
    Guid AccountId
    ) : AccountBase(AccountId);