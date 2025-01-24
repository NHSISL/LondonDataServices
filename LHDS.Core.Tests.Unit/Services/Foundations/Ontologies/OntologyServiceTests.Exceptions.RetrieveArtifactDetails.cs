// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologies
{
    public partial class OntologyServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveArtifactDetailsIfServiceErrorOccursAndLogItAsync()
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
                broker.GetArtifactDetailsAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> retrieveArtifactDetailTask =
                this.ontologyService.RetrieveArtifactDetailsAsync(someRelativeUrl);

            OntologyServiceException actualOntologyServiceException =
                await Assert.ThrowsAsync<OntologyServiceException>(
                    retrieveArtifactDetailTask.AsTask);

            // then
            actualOntologyServiceException.Should()
                .BeEquivalentTo(expectedOntologyServiceException);

            this.ontologyBrokerMock.Verify(broker =>
                broker.GetArtifactDetailsAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedOntologyServiceException))),
                        Times.Once);

            this.ontologyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}