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
        private readonly ILoggingBroker loggingBroker;

        public AddressLoadingAuditProcessingService(
            IAddressLoadingAuditService addressLoadingAuditService,
            ILoggingBroker loggingBroker)
        {
            this.addressLoadingAuditService = addressLoadingAuditService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<AddressLoadingAudit> AddAddressLoadingAuditAsync(AddressLoadingAudit addressLoadingAudit)
        {
            return await this.addressLoadingAuditService.AddAddressLoadingAuditAsync(addressLoadingAudit);
        }
    }
}
