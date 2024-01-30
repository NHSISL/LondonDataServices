// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Processings.AddressLoadingAudits;
using LHDS.Core.Services.Processings.AddressNormalisations;

namespace LHDS.Core.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationService : IAddressNormalisationOrchestrationService
    {
        private readonly IAddressParserService addressParserService;
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;
        private readonly IAddressLoadingAuditProcessingService addressLoadingAuditProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public AddressNormalisationOrchestrationService(
            IAddressParserService addressParserService,
            IAddressNormalisationProcessingService addressNormalisationProcessingService,
            IAddressLoadingAuditProcessingService addressLoadingAuditProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressParserService = addressParserService;
            this.addressNormalisationProcessingService = addressNormalisationProcessingService;
            this.addressLoadingAuditProcessingService = addressLoadingAuditProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<List<Address>> ProcessDataAsync(string data) =>
            throw new NotImplementedException();
    }
}
