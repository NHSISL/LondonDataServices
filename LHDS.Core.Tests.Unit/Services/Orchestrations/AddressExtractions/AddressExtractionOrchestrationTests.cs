// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        private readonly Mock<IAddressParserService> addressParserServiceMock;
        private readonly Mock<IAddressExtractionAuditService> addressExtractionAuditServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerrMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;

        public AddressExctractioneOrchestrationServiceTests()
        {
            this.addressParserServiceMock = new Mock<IAddressParserService>();
            this.addressExtractionAuditServiceMock = new Mock<IAddressExtractionAuditService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerrMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.addressExtractionOrchestrationService = new AddressExtractionOrchestrationService(
                addressParserService: addressParserServiceMock.Object,
                addressExtractionAuditService: addressExtractionAuditServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerrMock.Object);
        }
    }
}
