using System;

namespace LHDS.Landings.Client.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInformation(string message);
        void LogTrace(string message);
    }
}
