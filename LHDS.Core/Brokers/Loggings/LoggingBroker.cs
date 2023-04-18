// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Extensions.Exceptions;
using Microsoft.Extensions.Logging;

namespace LHDS.Core.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> logger;

        public LoggingBroker(ILogger<LoggingBroker> logger) =>
            this.logger = logger;

        public void LogInformation(string message) =>
            this.logger.LogInformation(message);

        public void LogTrace(string message) =>
            this.logger.LogTrace(message);

        public void LogDebug(string message) =>
            this.logger.LogDebug(message);

        public void LogWarning(string message) =>
            this.logger.LogWarning(message);

        public void LogError(Exception exception) =>
            this.logger.LogError(exception, $"{exception.Message} {exception.GetValidationSummary()}");

        public void LogCritical(Exception exception) =>
            this.logger.LogCritical(exception, $"{exception.Message} {exception.GetValidationSummary()}");
    }
}
