// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.ConfigImportExportTool.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInformation(string message);
        ValueTask LogInformationAsync(string message);
        void LogTrace(string message);
        ValueTask LogTraceAsync(string message);
        void LogDebug(string message);
        ValueTask LogDebugAsync(string message);
        void LogWarning(string message);
        ValueTask LogWarningAsync(string message);
        void LogError(Exception exception);
        ValueTask LogErrorAsync(Exception exception);
        void LogCritical(Exception exception);
        ValueTask LogCriticalAsync(Exception exception);
    }
}
