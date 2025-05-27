// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Brokers.Telemetries
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    internal class PassThroughProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor next;

        public PassThroughProcessor(ITelemetryProcessor next)
        {
            this.next = next;
        }

        public void Process(ITelemetry item)
        {
            this.next.Process(item);
        }
    }
}
