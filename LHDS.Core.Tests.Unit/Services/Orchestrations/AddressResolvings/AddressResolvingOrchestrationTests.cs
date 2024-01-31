// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Models.Processings.AddressLoadingAudits.Exceptions;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Orchestrations.AddressResolvings;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressLoadingAudits;
using LHDS.Core.Services.Processings.AddressMatchers;
using LHDS.Core.Services.Processings.AddressNormalisations;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressResolvings
{
    public partial class AddressResolvingOrchestrationServiceTests
    {
        private readonly Mock<IAddressProcessingService> addressProcessingServiceMock;
        private readonly Mock<IAddressMatcherProcessingService> addressMatcherProcessingServiceMock;
        private readonly Mock<IResolvedAddressProcessingService> resolvedAddressProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressResolvingOrchestrationService addressResolvingOrchestrationService;

        public AddressResolvingOrchestrationServiceTests()
        {
            this.addressProcessingServiceMock = new Mock<IAddressProcessingService>();
            this.addressMatcherProcessingServiceMock = new Mock<IAddressMatcherProcessingService>();
            this.resolvedAddressProcessingServiceMock = new Mock<IResolvedAddressProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.addressResolvingOrchestrationService = new AddressResolvingOrchestrationService(
                addressProcessingService: addressProcessingServiceMock.Object,
                addressMatcherProcessingService: addressMatcherProcessingServiceMock.Object,
                resolvedAddressProcessingService: resolvedAddressProcessingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

    }
}
