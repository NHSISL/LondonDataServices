// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Configurations.Retries
{
    internal class RetryConfig : IRetryConfig
    {
        internal RetryConfig()
        {
            MaxRetryAttempts = 5;
            PauseBetweenFailures = TimeSpan.FromSeconds(1);
        }

        public RetryConfig(int maxRetryAttempts, TimeSpan pauseBetweenFailures)
        {
            MaxRetryAttempts = maxRetryAttempts;
            PauseBetweenFailures = pauseBetweenFailures;
        }

        public int MaxRetryAttempts { get; private set; }
        public TimeSpan PauseBetweenFailures { get; private set; }
    }
}
