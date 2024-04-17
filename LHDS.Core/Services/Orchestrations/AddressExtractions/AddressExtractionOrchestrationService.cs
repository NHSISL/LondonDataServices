// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressParsers;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService : IAddressExtractionOrchestrationService
    {
        private readonly IAddressParserService addressParserService;
        private readonly IAddressExtractionAuditService addressExtractionAuditService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public AddressExtractionOrchestrationService(
            IAddressParserService addressParserService,
            IAddressExtractionAuditService addressExtractionAuditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressParserService = addressParserService;
            this.addressExtractionAuditService = addressExtractionAuditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public async ValueTask<List<Address>> ProcessAddressesAsync(byte[] data) =>
           throw new NotImplementedException();

        public async ValueTask<List<ResolvedAddress>> ProcessResolvedAddressesAsync(byte[] data) =>
            throw new NotImplementedException();
    }
}
