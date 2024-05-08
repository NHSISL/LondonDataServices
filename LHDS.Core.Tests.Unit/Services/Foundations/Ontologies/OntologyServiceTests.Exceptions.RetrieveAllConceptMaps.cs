// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologies
{
    public partial class OntologyServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllConceptMapsIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someRelativeUrl = GetRandomString();
            var serviceException = new Exception();

            var failedOntologyServiceException =
                new FailedOntologyServiceException(
                    message: "Failed ontology service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedOntologyServiceException =
                new OntologyServiceException(
                    message: "Ontology service error occurred, please contact support.",
                    innerException: failedOntologyServiceException);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OntologyAssets> retrieveOntologyByRelativeUrlTask =
                this.ontologyService.RetrieveAllConceptMapsAsync(someRelativeUrl);

            OntologyServiceException actualOntologyServiceException =
                await Assert.ThrowsAsync<OntologyServiceException>(
                    retrieveOntologyByRelativeUrlTask.AsTask);

            // then
            actualOntologyServiceException.Should()
                .BeEquivalentTo(expectedOntologyServiceException);

            this.ontologyBrokerMock.Verify(broker =>
                broker.GetAllAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedOntologyServiceException))),
                        Times.Once);

            this.ontologyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}