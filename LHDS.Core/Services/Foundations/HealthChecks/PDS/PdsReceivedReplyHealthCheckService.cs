using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.PDS
{
    public class PdsReceivedReplyHealthCheckService : IPdsHealthItemService
    {
        public ValueTask<HealthCheckResult> GetHealthStatusAsync()
        {
            throw new NotImplementedException();
        }
    }
}
