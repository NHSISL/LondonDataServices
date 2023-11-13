// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedTerminologyArtifactProcessingDependencyValidationException =
                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Throws(dependencyValidationException);

            // when
            Action terminologyArtifactRetrieveAction = () =>
                this.terminologyArtifactProcessingService.RetrieveAllTerminologyArtifactsAsync();

            TerminologyArtifactProcessingDependencyValidationException actualException =
                Assert.Throws<TerminologyArtifactProcessingDependencyValidationException>(
                    terminologyArtifactRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingDependencyValidationException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyArtifactProcessingDependencyValidationException))),
                         Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
