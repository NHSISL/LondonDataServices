// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    public interface ISubscriberCredentialOrchestration
    {
        /// <summary>
        /// Method to update or add a subscriber credential
        /// </summary>
        /// <param name="subscriberCredential">The subscriber credentials</param>
        /// <param name="regenerateKeys">If TRUE, the service will regenerate the cryptography keys</param>
        /// <param name="externalUse">If TRUE, the cryptography private keys will be omitted from the result</param>
        /// <returns></returns>
        ValueTask<SubscriberCredential> ModifyOrAddSubscriberCredentialAsync(
            SubscriberCredential subscriberCredential, bool regenerateKeys = false, bool externalUse = true);

        ValueTask<IQueryable<SubscriberCredential>> RetrieveAllSubscriberCredentialsAsync();
        ValueTask<List<Guid>> RetrieveAllActiveSubscriberCredentialIdsAsync(Guid supplierId);

        /// <summary>
        /// Method to retrieve a subscriber credential by its id
        /// </summary>
        /// <param name="subscriberCredentialId">The subscriber credential id</param>
        /// <param name="externalUse">If TRUE, the cryptography private keys will be omitted from the result</param>
        /// <returns></returns>
        ValueTask<SubscriberCredential> RetrieveSubscriberCredentialByIdAsync(
            Guid subscriberCredentialId,
            bool externalUse = true);

        ValueTask RemoveSubscriberCredentialByIdAsync(Guid subscriberCredentialId);
    }
}
