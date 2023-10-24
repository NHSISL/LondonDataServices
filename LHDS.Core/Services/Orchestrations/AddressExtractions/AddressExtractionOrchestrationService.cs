// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    internal class AddressExtractionOrchestrationService : IAddressExtractionOrchestrationService
    {
        private readonly IAddressParserService addressParserService;
        private readonly IDocumentService documentService;
        private readonly IAddressExtractionAuditService addressExtractionAuditService;
        private readonly ILoggingBroker loggingBroker;

        public AddressExtractionOrchestrationService(
            IAddressParserService addressParserService,
            IDocumentService documentService,
            IAddressExtractionAuditService addressExtractionAuditService,
            ILoggingBroker loggingBroker)
        {
            this.addressParserService = addressParserService;
            this.documentService = documentService;
            this.addressExtractionAuditService = addressExtractionAuditService;
            this.loggingBroker = loggingBroker;
        }

        public async Task<List<Address>> ProcessDataAsync(byte[] data) =>
            throw new NotImplementedException();
    }
}
