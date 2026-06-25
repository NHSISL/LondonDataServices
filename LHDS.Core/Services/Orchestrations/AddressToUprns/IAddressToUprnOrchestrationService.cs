// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.AddressToUprns
{
    public interface IAddressToUprnOrchestrationService
    {
        ValueTask MatchAddressToUprnAsync(
            Stream input,
            string fileName,
            Guid correlationId,
            CancellationToken cancellationToken = default);
    }
}