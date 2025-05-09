// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks
{
    public interface IHealthItemCheckService
    {
        ValueTask<HealthCheckResult> GetHealthStatusAsync();
    }
}
