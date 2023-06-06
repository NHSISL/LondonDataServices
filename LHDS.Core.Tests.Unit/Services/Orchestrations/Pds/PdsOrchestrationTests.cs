// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Orchestrations.Pds;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

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
        private readonly PdsConfiguration pdsConfiguration;
        private readonly ICompareLogic compareLogic;
        private readonly IPdsOrchestrationService pdsOrchestrationService;
        private readonly IConfiguration inMemoryConfiguration;

        public PdsOrchestrationTests()
        {
            var appSettingsStub = new Dictionary<string, string> {
                { "OptOutSettings:ExpiredAfterDays", "7" },
                { "OptOutSettings:InputFolder", GetRandomString() },
                { "OptOutSettings:OptOutFileHasHeader", "false" },
                { "OptOutSettings:OutputFolder", GetRandomString() },
                { "OptOutSettings:OptOutFileRequireTrailingComma", "true" },
                { "OptOutSettings:To", GetRandomString() },
                { "OptOutSettings:WorkflowId", GetRandomString() },
                { "MeshConfiguration:MailboxId", GetRandomString() },
                { "MeshConfiguration:Password", GetRandomString() },
                { "MeshConfiguration:Key", GetRandomString() },
                { "MeshConfiguration:Url", GetRandomString() },
                { "MeshConfiguration:MexClientVersion", GetRandomString() },
                { "MeshConfiguration:MexOSName", GetRandomString() },
                { "MeshConfiguration:MexOSVersion", GetRandomString() },
                { "MeshConfiguration:RootCertificate", null },
                { "MeshConfiguration:IntermediateCertificates", null },
                { "MeshConfiguration:ClientCertificate", null },
                { "MeshConfiguration:WorkflowId", GetRandomString() }
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
             .AddInMemoryCollection(appSettingsStub)
             .Build();

            this.pdsConfiguration = new PdsConfiguration
            {
                To = inMemoryConfiguration["OptOutSettings:InputFolder"],
                WorkflowId = inMemoryConfiguration["OptOutSettings:WorkflowId"],
            };

            this.pdsAuditServiceMock = new Mock<IPdsAuditService>();
            this.documentServiceMock = new Mock<IDocumentService>();
            this.meshServiceMock = new Mock<IMeshService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();

            this.pdsOrchestrationService = new PdsOrchestrationService(
                pdsAuditService: pdsAuditServiceMock.Object,
                documentService: documentServiceMock.Object,
                meshService: meshServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                pdsConfiguration: pdsConfiguration
                );
        }
        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
         new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private static PdsAudit GetRandomPdsAudit(
            Guid identifier,
            Guid correlationIdentifier,
            string fileName,
            DateTimeOffset randomDate,
            string messageId)
        {
            PdsAudit pdsAudit = new PdsAudit
            {
                Id = identifier,
                CorrelationId = correlationIdentifier,
                FileName = fileName,
                Message = $"Sent message to mesh with id {messageId}",
                CreatedDate = randomDate,
                UpdatedDate = randomDate,
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            return pdsAudit;
        }

        private Expression<Func<PdsAudit, bool>> SamePdsAuditAs(
           PdsAudit expectedPdsAudit)
        {
            return actualPdsAudit =>
                this.compareLogic.Compare(expectedPdsAudit, actualPdsAudit)
                    .AreEqual;
        }
    }
}
