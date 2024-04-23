// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IResolvedAddressProcessingService> resolvedAddressProcessingServiceMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;

        public ResolvedAddressOrchestrationTests()
        {
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.resolvedAddressProcessingServiceMock = new Mock<IResolvedAddressProcessingService>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.resolvedAddressOrchestrationService = new ResolvedAddressOrchestrationService(
                documentProcessingService: this.documentProcessingServiceMock.Object,
                resolvedAddressProcessingService: this.resolvedAddressProcessingServiceMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
