// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Coordinations.EmisLandings;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        private readonly Mock<ISubscriberCredentialOrchestration> subscriberCredentialOrchestrationMock;
        private readonly Mock<IEmisLandingOrchestrationService> emisLandingOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IEmisLandingCoordinationService emisLandingCoordinationService;

        public EmisLandingCoordinationServiceTests()
        {
            this.subscriberCredentialOrchestrationMock = new Mock<ISubscriberCredentialOrchestration>();
            this.emisLandingOrchestrationServiceMock = new Mock<IEmisLandingOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.emisLandingCoordinationService = new EmisLandingCoordinationService(
                subscriberCredentialOrchestration: subscriberCredentialOrchestrationMock.Object,
                emisLandingOrchestrationService: emisLandingOrchestrationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static List<string> GetRandomStrings(int count)
        {
            var messages = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var message = GetRandomString();
                messages.Add(message);
            }

            return messages;
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private static Expression<Func<Xeption, bool>> IsSameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static List<Guid> CreateRandomActiveSubscriberAgreementIds(int number)
        {
            return Enumerable.Range(0, number)
                .Select(_ => Guid.NewGuid())
                .ToList();
        }

        private static List<string> CreateRandomLandingPaths(int number)
        {
            return Enumerable.Range(0, number)
                .Select(_ => GetRandomString())
                .ToList();
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
           new DateTimeRange(earliestDate: new DateTime()).GetValue();

        public static List<SubscriberCredential> CreateRandomSubscriberCredentials(
            List<Guid> subscriberAgreementIds)
        {
            List<SubscriberCredential> subscriberCredentials = new List<SubscriberCredential>();

            foreach (Guid subscriberAgreementId in subscriberAgreementIds)
            {
                subscriberCredentials.Add(CreateRandomSubscriberCredentialsFiller(subscriberAgreementId).Create());
            }

            return subscriberCredentials;
        }

        public static Filler<SubscriberCredential> CreateRandomSubscriberCredentialsFiller(Guid subscriberAgreementId)
        {
            DateTimeOffset dateTimeOffset = new DateTimeRange(earliestDate: new DateTime()).GetValue();
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(() => subscriberAgreementId);

            return filler;
        }

        public static SubscriberCredential CreateRandomSubscriberCredential(Guid subscriberAgreementId)
        {
            DateTimeOffset dateTimeOffset = new DateTimeRange(earliestDate: new DateTime()).GetValue();
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(() => subscriberAgreementId);

            return filler.Create();
        }

        public static string CreateRandomSubscriberCredentialIdFileName(Guid subscriberAgreementId)
        {
            string fileName = "";

            for (int i = 0; i < 10; i++)
            {
                if (i != 5)
                {
                    fileName = fileName + GetRandomString() + "/";
                }
                else
                {
                    fileName = fileName + subscriberAgreementId.ToString() + "/";
                }
            }

            return fileName;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset,
            Guid subscriberAgreementId)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(() =>
                    CreateRandomFilePath(subscriberAgreementId));

            return filler;
        }

        private static IngestionTracking CreateRandomIngestionTracking(
            DateTimeOffset dateTimeOffset,
            Guid subscriberAgreementId) =>
            CreateIngestionTrackingFiller(dateTimeOffset, subscriberAgreementId).Create();

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking expectedIngestionTracking)
        {
            return actualIngestionTracking =>
                this.compareLogic.Compare(expectedIngestionTracking, actualIngestionTracking)
                    .AreEqual;
        }

        private static string CreateRandomFilePath(Guid identifier)
        {
            return $"emisnightingale-data-preprod-provider-extracts" +
                $"/IM1" +
                $"/sftp" +
                $"/{identifier}" +
                $"/{DateTime.Now.ToString("yyyyMMdd")}" +
                $"/delta{GetRandomString()}" +
                $"_{GetRandomNumber(min: 2, max: 1000)}" +
                $"_Admin" +
                $"_Location" +
                $"_{DateTime.Now.ToString("yyyyMMddHHmmss")}" +
                $"_{identifier}.csv.gpg";
        }

        private static Guid GetLastRandomGuid(string filename)
        {
            int underscoreIndex = filename.LastIndexOf('_');
            int dotCsvIndex = filename.LastIndexOf(".csv.gpg");

            if (underscoreIndex != -1 && dotCsvIndex != -1 && underscoreIndex < dotCsvIndex)
            {
                int guidLength = dotCsvIndex - underscoreIndex - 1;
                string guidString = filename.Substring(underscoreIndex + 1, guidLength);

                if (Guid.TryParse(guidString, out Guid resultGuid))
                {
                    return resultGuid;
                }
            }

            return Guid.Empty;
        }

        private static List<Document> CreateRandomDocuments()
        {
            return CreateDocumentFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Document CreateRandomDocument() =>
            CreateDocumentFiller().Create();

        private static Filler<Document> CreateDocumentFiller()
        {
            var filler = new Filler<Document>();
            filler.Setup();

            return filler;
        }

        private Expression<Func<SubscriberCredential, bool>> SameSubscriberCredentialAs(
            SubscriberCredential expectedSubscriberCredential)
        {
            return actualSubscriberCredential =>
                this.compareLogic.Compare(expectedSubscriberCredential, actualSubscriberCredential)
                    .AreEqual;
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occured, please try again",
                    innerException),

                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "please try again.",

                    innerException),

                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation error occured, please try again.",
                    innerException),

                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, " +
                        "please try again.",

                    innerException),
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occured, please try again.",
                    innerException),

                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, please contact support.",
                    innerException),

                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occured, please try again.",
                    innerException),

                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, please contact support.",
                    innerException)
            };
        }
    }
}
