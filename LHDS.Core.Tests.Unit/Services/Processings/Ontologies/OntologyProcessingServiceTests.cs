// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hl7.Fhir.Model;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using LHDS.Core.Services.Foundations.Ontologies;
using LHDS.Core.Services.Processings.Ontologies;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Ontologies
{
    public partial class OntologyProcessingServiceTests
    {
        private readonly Mock<IOntologyService> ontologyServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOntologyProcessingService ontologyProcessingService;

        public OntologyProcessingServiceTests()
        {
            this.ontologyServiceMock = new Mock<IOntologyService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.ontologyProcessingService = new OntologyProcessingService(
                ontologyService: this.ontologyServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static OntologyAssets CreateRandomOntologys()
        {
            return new OntologyAssets
            {
                Assets = CreateRandomOntologyAssets(),
                NextPage = GetRandomString()
            };
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

        private static List<dynamic> CreateRandomArtifactProperties(string artifactType)
        {
            return Enumerable.Range(1, GetRandomNumber())
                .Select(item =>
                {
                    return new
                    {
                        FullUrl = GetRandomString(),
                        ResourceType = artifactType,
                        Version = GetRandomString(),
                        Name = GetRandomString(),
                        Title = GetRandomString(),
                        Status = "active",
                        LastUpdated = GetRandomDateTimeOffset()
                    };
                })
                .ToList<dynamic>();
        }

        private static OntologyAssets CreateArtiFactFromRandomData(
           List<dynamic> randomArtifactProperties,
           string nextPageUrl)
        {
            var ontologyAssets = new OntologyAssets
            {
                Assets = new List<OntologyAsset>(),
                NextPage = nextPageUrl
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
            }

            return ontologyAssets;
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OntologyValidationException(
                    message: "Ontology validation errors occurred, please try again.", innerException),

                new OntologyDependencyValidationException(
                    message: "Ontology dependency validation occurred, please try again.", innerException)
            };
        }
    }
}