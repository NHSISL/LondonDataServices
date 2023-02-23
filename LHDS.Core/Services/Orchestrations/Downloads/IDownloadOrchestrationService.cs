// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public interface IDownloadOrchestrationService
    {
        ValueTask ProcessAsync();
        ValueTask ProcessAsync(string fileName);
    }
}