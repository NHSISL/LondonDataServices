// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface ILandingClient
    {
        ValueTask<List<string>> ProcessAsync();
        ValueTask<string> ProcessAsync(string fileName);
    }
}
