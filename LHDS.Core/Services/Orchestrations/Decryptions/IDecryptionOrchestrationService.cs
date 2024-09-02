// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public interface IDecryptionOrchestrationService
    {
        ValueTask<(string DecryptedFileName, Guid IngestionTrackingId)> DecryptAsync(
            string encryptedFileName,
            SubscriberCredential subscriberCredential);

        ValueTask<string?> GetNextItemToBeDecrypted();
    }
}
