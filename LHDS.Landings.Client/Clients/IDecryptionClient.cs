// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Clients
{
    public interface IDecryptionClient
    {
        ValueTask DecryptAsync(string fileName);
    }
}
