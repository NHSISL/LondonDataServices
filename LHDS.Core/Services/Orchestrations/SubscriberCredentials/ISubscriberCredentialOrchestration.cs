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
        ValueTask<SubscriberCredential> ModifyOrAddSubscriberCredentialAsync(SubscriberCredential subscriberCredential);
        IQueryable<SubscriberCredential> RetrieveAllSubscriberCredentials();
        ValueTask<List<Guid>> RetrieveAllActiveSubscriberCredentialIds();

        ValueTask<SubscriberCredential> RetrieveSubscriberCredentialByIdAsync(
            Guid subscriberCredentialId, 
            bool externalUse = true);

        ValueTask<SubscriberCredential> RemoveSubscriberCredentialByIdAsync(Guid subscriberCredentialId);
    }
}
