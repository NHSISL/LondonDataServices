// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        private readonly Mock<IAddressParserService> addressParserServiceMock;
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly Mock<IAddressExtractionAuditService> addressExtractionAuditServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;

        public AddressExctractioneOrchestrationServiceTests()
        {
            this.addressParserServiceMock = new Mock<IAddressParserService>();
            this.documentServiceMock = new Mock<IDocumentService>();
            this.addressExtractionAuditServiceMock = new Mock<IAddressExtractionAuditService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.addressExtractionOrchestrationService = new AddressExtractionOrchestrationService(
                addressParserService: addressParserServiceMock.Object,
                documentService: documentServiceMock.Object,
                addressExtractionAuditService: addressExtractionAuditServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
