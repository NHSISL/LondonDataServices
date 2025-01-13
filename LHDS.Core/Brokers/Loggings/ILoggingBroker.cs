// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInformation(string message);
        ValueTask LogInformationAsync(string message);
        ValueTask LogTraceAsync(string message);
        ValueTask LogDebugAsync(string message);
        ValueTask LogWarningAsync(string message);
        void LogError(Exception exception);
        ValueTask LogErrorAsync(Exception exception);
        void LogCritical(Exception exception);
        ValueTask LogCriticalAsync(Exception exception);
    }
}
