// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Services.Orchestrations.Downloads
{
    public interface IDownloadOrchestrationService
    {
        ValueTask ProcessAsync();
    }
}