// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologys
{
    public partial class OntologyServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllCodingSystemsIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someRelativeUrl = GetRandomString();
            var serviceException = new Exception();

            var failedOntologyServiceException =
                new FailedOntologyServiceException(
                    message: "Failed ontology service occurred, please contact support",
                    innerException: serviceException);

            var expectedOntologyServiceException =
                new OntologyServiceException(
                    message: "Ontology service error occurred, contact support.",
                    innerException: failedOntologyServiceException);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllCodingSystemsAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OntologyAssets> retrieveOntologyByRelativeUrlTask =
                this.ontologyService.RetrieveAllCodingSystemsAsync(someRelativeUrl);

            OntologyServiceException actualOntologyServiceException =
                await Assert.ThrowsAsync<OntologyServiceException>(
                    retrieveOntologyByRelativeUrlTask.AsTask);

            // then
            actualOntologyServiceException.Should()
                .BeEquivalentTo(expectedOntologyServiceException);

            this.ontologyBrokerMock.Verify(broker =>
                broker.GetAllCodingSystemsAsync(It.IsAny<string>()),
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