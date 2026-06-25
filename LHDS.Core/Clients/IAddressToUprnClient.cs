// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface IAddressToUprnClient
    {
        ValueTask MatchAddressToUprnAsync(
            Stream data,
            string filename,
            Guid correlationId,
            CancellationToken cancellationToken = default);
    }
}
