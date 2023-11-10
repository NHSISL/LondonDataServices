// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.Core.Models.Configurations.Retries
{
    internal interface IRetryConfig
    {
        int MaxRetryAttempts { get; }
        TimeSpan PauseBetweenFailures { get; }
    }
}
