// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Processings.SecureDatas
{
    public interface ISecureDataProcessingService
    {
        ValueTask<SubscriberCredential> AddOrModifySecureDataAsync(SubscriberCredential subscriberCredential);

        ValueTask<SubscriberCredential> RetrieveSecretsByKeyVaultKeyNameAsync(
            SubscriberCredential subscriberCredential);

        ValueTask<SubscriberCredential> RemoveSecureDataAsync(SubscriberCredential subscriberCredential);
    }
}