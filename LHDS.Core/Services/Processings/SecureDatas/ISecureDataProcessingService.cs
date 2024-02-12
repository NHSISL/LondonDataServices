// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public interface ISecureDataProcessingService
    {
        ValueTask<SubscriberCredential> AddOrModifySecureData(SubscriberCredential secureData);
        ValueTask<SubscriberCredential> RetrieveSecretsBySubscriberAgreementNameAsync(string subscriberAgreementName);
        ValueTask<SubscriberCredential> RemoveSecureData(SubscriberCredential secureData);
    }
}