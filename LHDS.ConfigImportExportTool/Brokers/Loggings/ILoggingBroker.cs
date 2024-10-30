// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.ConfigImportExportTool.Brokers.Loggings
{
    internal interface ILoggingBroker
    {
        ValueTask LogInformationAsync(string message);
        ValueTask LogTraceAsync(string message);
        ValueTask LogDebugAsync(string message);
        ValueTask LogWarningAsync(string message);
        ValueTask LogErrorAsync(Exception exception);
        ValueTask LogCriticalAsync(Exception exception);
    }
}
