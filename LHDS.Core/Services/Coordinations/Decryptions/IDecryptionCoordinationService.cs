// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Coordinations.Decryptions
{
    public interface IDecryptionCoordinationService
    {
        ValueTask<string> DecryptAsync(string encryptedFileName);
        ValueTask RetryDecryptOnAllAsync();
        ValueTask ProcessDecryptedItemsForBatchCompleteAsync();
    }
}