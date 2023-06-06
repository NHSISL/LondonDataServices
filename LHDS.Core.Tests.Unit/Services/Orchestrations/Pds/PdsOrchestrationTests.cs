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
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Orchestrations.Pds;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

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
                { "PdsSettings:InputFolder", GetRandomString() },
                { "PdsSettings:PdsFileHasHeader", "false" },
                { "PdsSettings:OutputFolder", GetRandomString() },
                { "PdsSettings:PdsFileRequireTrailingComma", "true" },
                { "PdsSettings:To", GetRandomString() },
                { "PdsSettings:WorkflowId", GetRandomString() },
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
                InputFolder = inMemoryConfiguration["PdsSettings:InputFolder"],
                PdsFileHasHeader = bool.Parse(inMemoryConfiguration["PdsSettings:PdsFileHasHeader"]),
                OutputFolder = inMemoryConfiguration["PdsSettings:OutputFolder"],

                PdsFileRequireTrailingComma =
                    bool.Parse(inMemoryConfiguration["PdsSettings:PdsFileRequireTrailingComma"]),

                To = inMemoryConfiguration["PdsSettings:InputFolder"],
                WorkflowId = inMemoryConfiguration["PdsSettings:WorkflowId"],

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

        public static TheoryData PdsDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new PdsOrchestrationValidationException(innerException),
                new PdsOrchestrationDependencyValidationException(innerException),
                new DocumentValidationException(innerException),
                new DocumentDependencyValidationException(innerException),
                new MeshValidationException(innerException),
                new MeshDependencyValidationException(innerException),
            };
        }

        public static TheoryData PdsDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new PdsOrchestrationDependencyException(innerException),
                new PdsOrchestrationServiceException(innerException),
                new DocumentDependencyException(innerException),
                new DocumentServiceException(innerException),
                new MeshDependencyException(innerException),
                new MeshServiceException(innerException),
            };
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
