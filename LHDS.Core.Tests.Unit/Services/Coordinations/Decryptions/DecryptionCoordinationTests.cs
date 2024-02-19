// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Decryptions;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        private readonly Mock<ISubscriberCredentialOrchestration> subscriberCredentialOrchestrationMock;
        private readonly Mock<IDecryptionOrchestrationService> decryptionOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IDecryptionCoordinationService decryptionCoordinationService;

        public DecryptionCoordinationServiceTests()
        {
            this.subscriberCredentialOrchestrationMock = new Mock<ISubscriberCredentialOrchestration>();
            this.decryptionOrchestrationServiceMock = new Mock<IDecryptionOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.decryptionCoordinationService = new DecryptionCoordinationService(
                subscriberCredentialOrchestration: subscriberCredentialOrchestrationMock.Object,
                decryptionOrchestrationService: decryptionOrchestrationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

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

        //private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        //{
        //    var filler = new Filler<IngestionTracking>();

        //    filler.Setup()
        //        .OnType<DateTimeOffset>().Use(dateTimeOffset)
        //        .OnType<DateTimeOffset?>().Use(dateTimeOffset)
        //        .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
        //        .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
        //        .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(() => GenerateFilename());

        //    return filler;
        //}

        //private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
        //    CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking expectedIngestionTracking)
        {
            return actualIngestionTracking =>
                this.compareLogic.Compare(expectedIngestionTracking, actualIngestionTracking)
                    .AreEqual;
        }

        private static string CreateRandomFilePath(Guid identifier)
        {
            return $"{identifier}/0122235/{GetRandomNumber}_{GetRandomString()}_{GetRandomString()}_{GetRandomNumber()}_{identifier}.csv.gpg;";
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
    }
}
