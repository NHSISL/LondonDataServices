// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressLoadingAudits;
using LHDS.Core.Services.Processings.AddressNormalisations;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationService : IAddressPersistanceOrchestrationService
    {
        private readonly IAddressProcessingService addressProcessingService;
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;
        private readonly IAddressLoadingAuditProcessingService auditProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AddressPersistanceOrchestrationService(
            IAddressProcessingService addressProcessingService,
            IAddressNormalisationProcessingService addressNormalisationProcessingService,
            IAddressLoadingAuditProcessingService auditProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.addressProcessingService = addressProcessingService;
            this.addressNormalisationProcessingService = addressNormalisationProcessingService;
            this.auditProcessingService = auditProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask ProcessAsync(Address address)
        {
                this.addressNormalisationProcessingService.GetNormalisedAddress(address.ToString());

                this.addressProcessingService.ModifyOrAddAddressAsync(address);

                var addressLoadingAudit = new AddressLoadingAudit
                {
                    Id = Guid.NewGuid(),    
                    CorrelationId = Guid.NewGuid(),
                    FileName = "ProcessedAddressDetails",   
                    Message = "Address processed successfully",
                    MessageId = "Success",  
                    CreatedBy = "System",   
                    UpdatedBy = "System", 
                    UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset(),
                    CreatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset()
                };

                this.auditProcessingService.AddAddressLoadingAuditAsync(addressLoadingAudit);

                return ValueTask.CompletedTask;
        }
    }
}
