// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Services.Foundations.Decryptions
{
    public interface IDecryptionService
    {
        Task<byte[]> DecryptAsync(byte[] data);
    }
}