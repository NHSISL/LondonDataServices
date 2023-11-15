// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hl7.Fhir.Model;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Services.Foundations.Ontologies;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologies
{
    public partial class OntologyServiceTests
    {
        private readonly Mock<IOntologyBroker> ontologyBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOntologyService ontologyService;

        public OntologyServiceTests()
        {
            this.ontologyBrokerMock = new Mock<IOntologyBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.ontologyService = new OntologyService(
                ontologyBroker: this.ontologyBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

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

        private static Bundle CreateCodeSystemBundleFromRandomData(
            List<dynamic> randomArtifactProperties,
            string nextPageUrl)
        {
            Bundle externalBundleResult = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Type = Bundle.BundleType.Searchset,
                Total = randomArtifactProperties.Count,
                Link = new List<Bundle.LinkComponent>
                {
                    new Bundle.LinkComponent
                    {
                        Relation = "self",
                        Url = "http://localhost:5000/api/fhir/ValueSet"
                    },
                    new Bundle.LinkComponent
                    {
                        Relation = "next",
                        Url = nextPageUrl
                    }
                },
                Entry = new List<Bundle.EntryComponent>()
            };

            foreach (var item in randomArtifactProperties)
            {
                externalBundleResult.Entry.Add(
                    new Bundle.EntryComponent
                    {
                        FullUrl = item.FullUrl,
                        Resource = new CodeSystem
                        {
                            Meta = new Meta
                            {
                                LastUpdated = item.LastUpdated,
                            },

                            Version = item.Version,
                            Name = item.Name,
                            Title = item.Title,

                            Status = (PublicationStatus)Enum.Parse(
                                typeof(PublicationStatus), item.Status, ignoreCase: true),
                        }
                    });
            }

            return externalBundleResult;
        }

        private static Bundle CreateValueSetBundleFromRandomData(
            List<dynamic> randomArtifactProperties,
            string nextPageUrl)
        {
            Bundle externalBundleResult = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Type = Bundle.BundleType.Searchset,
                Total = randomArtifactProperties.Count,
                Link = new List<Bundle.LinkComponent>
                {
                    new Bundle.LinkComponent
                    {
                        Relation = "self",
                        Url = "http://localhost:5000/api/fhir/ValueSet"
                    },
                    new Bundle.LinkComponent
                    {
                        Relation = "next",
                        Url = nextPageUrl
                    }
                },
                Entry = new List<Bundle.EntryComponent>()
            };

            foreach (var item in randomArtifactProperties)
            {
                externalBundleResult.Entry.Add(
                    new Bundle.EntryComponent
                    {
                        FullUrl = item.FullUrl,
                        Resource = new ValueSet
                        {
                            Meta = new Meta
                            {
                                LastUpdated = item.LastUpdated,
                            },

                            Version = item.Version,
                            Name = item.Name,
                            Title = item.Title,

                            Status = (PublicationStatus)Enum.Parse(
                                typeof(PublicationStatus), item.Status, ignoreCase: true),
                        }
                    });
            }

            return externalBundleResult;
        }

        private static Bundle CreateConceptMapBundleFromRandomData(
            List<dynamic> randomArtifactProperties,
            string nextPageUrl)
        {
            Bundle externalBundleResult = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Type = Bundle.BundleType.Searchset,
                Total = randomArtifactProperties.Count,
                Link = new List<Bundle.LinkComponent>
                {
                    new Bundle.LinkComponent
                    {
                        Relation = "self",
                        Url = "http://localhost:5000/api/fhir/ValueSet"
                    },
                    new Bundle.LinkComponent
                    {
                        Relation = "next",
                        Url = nextPageUrl
                    }
                },
                Entry = new List<Bundle.EntryComponent>()
            };

            foreach (var item in randomArtifactProperties)
            {
                externalBundleResult.Entry.Add(
                    new Bundle.EntryComponent
                    {
                        FullUrl = item.FullUrl,
                        Resource = new ConceptMap
                        {
                            Meta = new Meta
                            {
                                LastUpdated = item.LastUpdated,
                            },

                            Version = item.Version,
                            Name = item.Name,
                            Title = item.Title,

                            Status = (PublicationStatus)Enum.Parse(
                                typeof(PublicationStatus), item.Status, ignoreCase: true),
                        }
                    });
            }

            return externalBundleResult;
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
    }
}