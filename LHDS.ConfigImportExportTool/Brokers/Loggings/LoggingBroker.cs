// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Extensions.Exceptions;
using Microsoft.Extensions.Logging;

namespace LHDS.ConfigImportExportTool.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> logger;

        public LoggingBroker(ILogger<LoggingBroker> logger) =>
            this.logger = logger;

        public void LogInformation(string message) =>
            logger.LogInformation(message);

        public async ValueTask LogInformationAsync(string message) =>
            logger.LogInformation(message);

        public void LogTrace(string message) =>
            logger.LogTrace(message);

        public async ValueTask LogTraceAsync(string message) =>
            logger.LogTrace(message);

        public void LogDebug(string message) =>
            logger.LogDebug(message);

        public async ValueTask LogDebugAsync(string message) =>
            logger.LogDebug(message);

        public void LogWarning(string message) =>
            logger.LogWarning(message);

        public async ValueTask LogWarningAsync(string message) =>
            logger.LogWarning(message);

        public void LogError(Exception exception) =>
            logger.LogError(exception, $"{exception.Message} {exception.GetValidationSummary()}");

        public async ValueTask LogErrorAsync(Exception exception) =>
            logger.LogError(exception, $"{exception.Message} {exception.GetValidationSummary()}");

        public void LogCritical(Exception exception) =>
            logger.LogCritical(exception, $"{exception.Message} {exception.GetValidationSummary()}");

        public async ValueTask LogCriticalAsync(Exception exception) =>
            logger.LogCritical(exception, $"{exception.Message} {exception.GetValidationSummary()}");
    }
}
