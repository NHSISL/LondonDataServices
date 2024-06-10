// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;
using LHDS.Core.Services.Orchestrations.TerminologyMetadata;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyPolls;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationTests
    {
        private readonly Mock<ITerminologyPollProcessingService> terminologyPollProcessingServiceMock;
        private readonly Mock<ITerminologyArtifactProcessingService> terminologyArtifactProcessingServiceMock;
        private readonly Mock<IOntologyProcessingService> ontologyProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly OntologyConfiguration ontologyConfiguration;
        private readonly ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public TerminologyMetadataOrchestrationTests()
        {
            terminologyPollProcessingServiceMock = new Mock<ITerminologyPollProcessingService>();
            terminologyArtifactProcessingServiceMock = new Mock<ITerminologyArtifactProcessingService>();
            ontologyProcessingServiceMock = new Mock<IOntologyProcessingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            compareLogic = new CompareLogic();
            identifierBrokerMock = new Mock<IIdentifierBroker>();

            ontologyConfiguration = new OntologyConfiguration
            {
                TerminologyServerResourceRelativeUrl = "/authoring/fhir/{{resourceType}}?_lastUpdated=ge{{datestamp}}" +
                    "&_name=dm+dCOMBINATION_PACK_IND&_elements=name,title,url,version,status&_count=10"
            };

            terminologyMetadataOrchestrationService = new TerminologyMetadataOrchestrationService(
                terminologyPollProcessingService: terminologyPollProcessingServiceMock.Object,
                terminologyArtifactProcessingService: terminologyArtifactProcessingServiceMock.Object,
                ontologyProcessingService: ontologyProcessingServiceMock.Object,
                ontologyConfiguration: ontologyConfiguration,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
          new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Expression<Func<TerminologyArtifact, bool>> SameTerminologyArtifactAs(
            TerminologyArtifact expectedTerminologyArtifact)
        {
            return actualTerminologyArtifact =>
                this.compareLogic.Compare(expectedTerminologyArtifact, actualTerminologyArtifact)
                    .AreEqual;
        }

        private Expression<Func<TerminologyPoll, bool>> SameTerminologyPollAs(
            TerminologyPoll expectedTerminologyPoll)
        {
            return actualTerminologyPoll =>
                this.compareLogic.Compare(expectedTerminologyPoll, actualTerminologyPoll)
                    .AreEqual;
        }
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static TerminologyPoll CreateRandomTerminologyPoll(string resourceType, DateTimeOffset lastPoll) =>
            CreateTerminologyPollFiller(resourceType, lastPoll).Create();

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller(string resourceType, DateTimeOffset lastPoll)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(terminologyMetadata => terminologyMetadata.ResourceType).Use(resourceType)
                .OnProperty(terminologyMetadata => terminologyMetadata.LastPoll).Use(lastPoll);

            return filler;
        }

        private static List<OntologyAsset> CreateRandomOntologyAssets() =>
            CreateOntologyFiller().Create(count: GetRandomNumber()).ToList();

        private static Filler<OntologyAsset> CreateOntologyFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();

            var filler = new Filler<OntologyAsset>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static List<dynamic> CreateRandomArtifactProperties(
            string resourceType, DateTimeOffset dateTimeOffset, Guid id)
        {
            string user = GetRandomString();
            return Enumerable.Range(1, GetRandomNumber())
                .Select(item =>
                {
                    return new
                    {
                        Id = id,
                        FullUrl = GetRandomString(),
                        ResourceType = resourceType,
                        Version = GetRandomString(),
                        Name = GetRandomString(),
                        Title = GetRandomString(),
                        Status = "active",
                        LastUpdated = dateTimeOffset,
                        IsCore = false,
                        IsDownloaded = false,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        UpdatedDate = dateTimeOffset,
                        CreatedDate = dateTimeOffset,
                        NextPage = GetRandomString()
                    };
                })
                .ToList<dynamic>();
        }

        private static OntologyAssets CreateOntologyAssetFromRandomData(List<dynamic> randomArtifactProperties)
        {
            var ontologyAssets = new OntologyAssets
            {
                Assets = new List<OntologyAsset>(),
                NextPage = randomArtifactProperties.First().NextPage,
            };

            foreach (var item in randomArtifactProperties)
            {
                ontologyAssets.Assets.Add(
                    new OntologyAsset
                    {
                        FullUrl = item.FullUrl,
                        ResourceType = item.ResourceType,
                        Version = item.Version,
                        Name = item.Name,
                        Title = item.Title,
                        Status = item.Status,
                        LastUpdated = item.LastUpdated
                    });

                ontologyAssets.NextPage = item.NextPage;
            }

            return ontologyAssets;
        }

        private static List<TerminologyArtifact> CreateTerminologyArtiFactFromRandomData(
            List<dynamic> randomArtifactProperties)
        {
            List<TerminologyArtifact> terminologyArtifacts = new List<TerminologyArtifact>();

            foreach (var item in randomArtifactProperties)
            {
                terminologyArtifacts.Add(
                    new TerminologyArtifact
                    {
                        Id = item.Id,
                        FullUrl = item.FullUrl,
                        ResourceType = item.ResourceType,
                        Version = item.Version,
                        Name = item.Name,
                        Title = item.Title,
                        Status = item.Status,
                        LastUpdated = item.LastUpdated,
                        IsCore = item.IsCore,
                        IsDownloaded = item.IsDownloaded,
                        CreatedBy = item.CreatedBy,
                        UpdatedBy = item.UpdatedBy,
                        UpdatedDate = item.UpdatedDate,
                        CreatedDate = item.CreatedDate
                    });
            }

            return terminologyArtifacts;
        }

        public static TheoryData<Xeption> TerminologyMetadataOrchestrationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occured, please try again",
                    innerException),

                new TerminologyPollProcessingDependencyValidationException(
                    message: "Terminology poll processing dependency validation occurred, please try again.",
                    innerException),

                new TerminologyArtifactProcessingValidationException(
                    message: "Terminology artifact processing validation errors occurred, please try again.",
                    innerException),

                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation occurred, please try again.",
                    innerException),

                new OntologyProcessingValidationException(
                    message: "Ontology processing validation errors occurred, fix the errors and try again.",
                    innerException),

                new OntologyProcessingDependencyValidationException(
                    message: "Ontology processing dependency validation occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> TerminologyMetadataOrchestrationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TerminologyPollProcessingDependencyException(
                    message: "Terminology poll processing dependency error occurred, please contact support.",
                    innerException),

                new TerminologyPollProcessingServiceException(
                    message: "Terminology poll processing service error occurred, please contact support.",
                    innerException),

                new TerminologyArtifactProcessingDependencyException(
                    message: "Terminology artifact processing dependency error occurred, please contact support.",
                    innerException),

                new TerminologyArtifactProcessingServiceException(
                    message: "Terminology artifact processing service error occurred, please contact support.",
                    innerException),

                new OntologyProcessingDependencyException(
                    message: "Ontology processing dependency error occurred, please contact support.",
                    innerException),

                new OntologyProcessingServiceException(
                    message: "Ontology processing service error occurred, please contact support.",
                    innerException),
            };
        }
    }
}