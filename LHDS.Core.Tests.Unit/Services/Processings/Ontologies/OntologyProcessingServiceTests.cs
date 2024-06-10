// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
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

        private static OntologyAssets CreateRandomOntologies()
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
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();

            var filler = new Filler<OntologyAsset>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
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

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OntologyDependencyException(
                    message: "Ontology dependency error occurred, please try again.", innerException),

                new OntologyServiceException(
                    message: "Ontology service error occurred, please try again.", innerException)
            };
        }
    }
}