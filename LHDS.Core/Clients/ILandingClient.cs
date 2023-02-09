// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface ILandingClient
    {
        ValueTask ProcessAsync();
    }
}
