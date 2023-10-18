// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Services.Foundations.AddressLoadingAudits;

namespace LHDS.Core.Services.Processings.AddressLoadingAudits
{
    public class AddressLoadingAuditProcessingService : IAddressLoadingAuditProcessingService
    {
        private readonly IAddressLoadingAuditService addressLoadingAuditService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressLoadingAuditProcessingService(
            IAddressLoadingAuditService addressLoadingAuditService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.addressLoadingAuditService = addressLoadingAuditService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AddressLoadingAudit> AddAddressLoadingAuditAsync(AddressLoadingAudit addressLoadingAudit)
        {
            throw new System.NotImplementedException();
        }
    }
}
