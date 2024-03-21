// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public interface ICryptographyKeyService
    {
        ValueTask<CryptographicKey> GenerateKeysAsync(
            string cryptographyType,
            string? comment = "",
            string? passPhrase = "",
            string? userName = "",
            string? email = "");
    }
}
