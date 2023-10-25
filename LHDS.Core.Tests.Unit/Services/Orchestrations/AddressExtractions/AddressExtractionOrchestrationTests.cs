// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        private readonly Mock<IAddressParserService> addressParserServiceMock;
        private readonly Mock<IAddressExtractionAuditService> addressExtractionAuditServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;

        public AddressExctractioneOrchestrationServiceTests()
        {
            this.addressParserServiceMock = new Mock<IAddressParserService>();
            this.addressExtractionAuditServiceMock = new Mock<IAddressExtractionAuditService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.addressExtractionOrchestrationService = new AddressExtractionOrchestrationService(
                addressParserService: addressParserServiceMock.Object,
                addressExtractionAuditService: addressExtractionAuditServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);
    }
}
