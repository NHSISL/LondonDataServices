// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Orchestrations.Pds;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        private readonly Mock<IPdsAuditService> pdsAuditServiceMock;
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly Mock<IMeshService> meshServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly IPdsOrchestrationService pdsOrchestrationService;

        public PdsOrchestrationTests()
        {
            pdsAuditServiceMock = new Mock<IPdsAuditService>();
            documentServiceMock = new Mock<IDocumentService>();
            meshServiceMock = new Mock<IMeshService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            identifierBrokerMock = new Mock<IIdentifierBroker>();

            this.pdsOrchestrationService = new PdsOrchestrationService(
                pdsAuditService: pdsAuditServiceMock.Object,
                documentService: documentServiceMock.Object,
                meshService: meshServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object
                );
        }
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
         new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
