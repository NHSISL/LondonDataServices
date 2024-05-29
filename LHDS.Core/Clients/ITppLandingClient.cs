// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Clients
{
    public interface ITppLandingClient
    {
        ValueTask<Guid> ProcessAsync(Document fileName, Guid supplierId);
    }
}