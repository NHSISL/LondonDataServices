// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    internal interface ISubscriberCredentialOrchestration
    {
        ValueTask<SubscriberCredential> ModifyOrAddSubscriberCredentialAsync(SubscriberCredential address);
        IQueryable<SubscriberCredential> RetrieveAllSubscriberCredentials();
        ValueTask<SubscriberCredential> RetrieveSubscriberCredentialByIdAsync(Guid subscriberCredentialId);
        ValueTask<SubscriberCredential> RetrieveOrAddSubscriberByNameAsync(string subscriberAgreementName);
        ValueTask<SubscriberCredential> RemoveSubscriberCredentialByIdAsync(Guid addressId);
    }
}
