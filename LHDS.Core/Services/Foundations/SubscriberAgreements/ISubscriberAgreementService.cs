using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public interface ISubscriberAgreementService
    {
        ValueTask<SubscriberAgreement> AddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        IQueryable<SubscriberAgreement> RetrieveAllSubscriberAgreements();
        ValueTask<SubscriberAgreement> RetrieveSubscriberAgreementByIdAsync(Guid subscriberAgreementId);
    }
}