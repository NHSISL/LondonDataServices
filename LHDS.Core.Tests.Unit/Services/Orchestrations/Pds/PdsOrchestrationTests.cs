// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Mesh;
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
        private readonly BlobContainers blobContainers;
        private readonly ICompareLogic compareLogic;
        private readonly IPdsOrchestrationService pdsOrchestrationService;
        private readonly IConfiguration inMemoryConfiguration;

        public PdsOrchestrationTests()
        {
            var appSettingsStub = new Dictionary<string, string> {
                { "pdsSettings:inputFolder", GetRandomString() },
                { "pdsSettings:pdsFileHasHeader", "false" },
                { "pdsSettings:outputFolder", GetRandomString() },
                { "pdsSettings:pdsFileRequireTrailingComma", "true" },
                { "pdsSettings:to", GetRandomString() },
                { "pdsSettings:workflowId", GetRandomString() },
                { "meshConfiguration:mailboxId", GetRandomString() },
                { "meshConfiguration:password", GetRandomString() },
                { "meshConfiguration:key", GetRandomString() },
                { "meshConfiguration:url", GetRandomString() },
                { "meshConfiguration:mexClientVersion", GetRandomString() },
                { "meshConfiguration:mexOSName", GetRandomString() },
                { "meshConfiguration:mexOSVersion", GetRandomString() },
                { "meshConfiguration:rootCertificate", null },
                { "meshConfiguration:intermediateCertificates", null },
                { "meshConfiguration:clientCertificate", null },
                { "meshConfiguration:workflowId", GetRandomString() }
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
             .AddInMemoryCollection(appSettingsStub)
             .Build();

            this.pdsConfiguration = new PdsConfiguration
            {
                InputFolder = inMemoryConfiguration["pdsSettings:inputFolder"],
                PdsFileHasHeader = bool.Parse(inMemoryConfiguration["pdsSettings:pdsFileHasHeader"]),
                OutputFolder = inMemoryConfiguration["pdsSettings:outputFolder"],

                PdsFileRequireTrailingComma =
                    bool.Parse(inMemoryConfiguration["pdsSettings:pdsFileRequireTrailingComma"]),

                To = inMemoryConfiguration["pdsSettings:inputFolder"],
                WorkflowId = inMemoryConfiguration["pdsSettings:workflowId"],

            };

            this.blobContainers = new BlobContainers
            {
                Pds = "pds"
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
                blobContainers: blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                pdsConfiguration: pdsConfiguration
                );
        }

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                this.compareLogic.Compare(expectedStream, actualStream)
                    .AreEqual;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
         new MnemonicString().GetValue();

        private static List<string> GetRandomStrings(int count)
        {
            var stringList = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string messageId = GetRandomString();
                stringList.Add(messageId);
            }

            return stringList;
        }

        private static MeshMessage CreateRandomMeshMessage() =>
            CreateMeshMessageFiller().Create();

        private static Filler<MeshMessage> CreateMeshMessageFiller()
        {
            var filler = new Filler<MeshMessage>();

            filler.Setup().OnProperty(message => message.Headers)
                .Use(new Dictionary<string, List<string>>());

            return filler;
        }

        private static List<MeshMessage> GetRandomMessages(List<string> randomMessageIds, string mexWorkflowId)
        {
            var messages = new List<MeshMessage>();
            var fileName =
                    GetRandomString() + "_" + GetRandomString() + "_" + GetRandomString() + "_" + GetRandomString() + ".csv";

            randomMessageIds.ForEach(id =>
            {
                MeshMessage message = ComposeMessage.CreateMeshMessage(
                    mexTo: GetRandomString(),
                    mexWorkflowId,
                    fileContent: Encoding.ASCII.GetBytes(GetRandomString()),
                    mexSubject: GetRandomString(),
                    mexLocalId: Guid.NewGuid().ToString(),
                    mexFileName: fileName);

                message.MessageId = id;
                messages.Add(message);
            });

            return messages;
        }

        private Expression<Func<Document, bool>> SameDocumentAs(
          Document expectedDocument)
        {
            return actualDocument =>
                this.compareLogic.Compare(expectedDocument, actualDocument)
                    .AreEqual;
        }

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
                MessageId = messageId,
                CreatedDate = randomDate,
                UpdatedDate = randomDate,
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            return pdsAudit;
        }

        public static TheoryData<Xeption> PdsDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new PdsOrchestrationValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException),

                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException),

                new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException),

                new DocumentDependencyValidationException(
                    message: "Document dependency validation occurred, please try again.",
                    innerException),

                new MeshValidationException(
                    message: "Mesh validation errors occurred, please try again.",
                    innerException),

                new MeshDependencyValidationException(
                    message: "Mesh dependency validation occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> PdsDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException),

                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException),

                new DocumentDependencyException(
                    message: "Document dependency error occurred, please contact support.",
                    innerException),

                new DocumentServiceException(
                    message: "Document service error occurred, please contact support.",
                    innerException),

                new MeshDependencyException(
                    message: "Mesh dependency error occurred, please contact support.",
                    innerException),

                new MeshServiceException(
                    message: "Mesh service error occurred, please contact support.",
                    innerException),
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
