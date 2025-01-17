// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.TppLandings.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class TppLandingsCoordinationTests
    {
        [Theory]
        [MemberData(nameof(TppDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessIfDependencyValidationOccursAndLogItAsync(
             Xeption dependancyValidationException)
        {
            // given
            Stream randomData = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));
            Stream inputStream = randomData;
            string inputFileName = GetRandomString();
            Guid inputSupplierId = Guid.NewGuid();
            Guid ingestionTrackingId = Guid.NewGuid();

            var expectedDependencyException =
                new TppLandingCoordinationDependencyValidationException(

                    message: "TPP landing coordination dependency validation error occurred, " +
                        "fix the errors and try again.",

                    dependancyValidationException.InnerException as Xeption);

            this.tppLandingOrchestrationServiceMock.Setup(service =>
                service.ProcessAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<Guid>()))
                    .Throws(dependancyValidationException);

            // when
            ValueTask<Guid> processTask = this.tppLandingCoordinationService
                .ProcessAsync(input: inputStream, fileName: inputFileName, supplierId: inputSupplierId);

            TppLandingCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TppLandingCoordinationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.tppLandingOrchestrationServiceMock.Verify(service =>
                service.ProcessAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                       Times.Once);

            this.tppLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TppDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessIfDependencyExceptionOccursAndLogItAsync(
          Xeption dependancyException)
        {
            // given
            Stream randomData = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));
            Stream inputStream = randomData;
            string inputFileName = GetRandomString();
            Guid inputSupplierId = Guid.NewGuid();
            Guid ingestionTrackingId = Guid.NewGuid();

            var expectedDependencyException =
                new TppLandingCoordinationDependencyException(
                    message: "TPP landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.tppLandingOrchestrationServiceMock.Setup(service =>
                service.ProcessAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<Guid>()))
                    .Throws(dependancyException);

            // when
            ValueTask<Guid> processTask = this.tppLandingCoordinationService
                .ProcessAsync(input: inputStream, fileName: inputFileName, supplierId: inputSupplierId);

            TppLandingCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<TppLandingCoordinationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.tppLandingOrchestrationServiceMock.Verify(service =>
                service.ProcessAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.tppLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            Stream randomData = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));
            Stream inputStream = randomData;
            string inputFileName = GetRandomString();
            Guid inputSupplierId = Guid.NewGuid();
            Guid ingestionTrackingId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTppCoordinationServiceException =
                new FailedTppLandingCoordinationServiceException(
                    message: "Failed TPP landing coordination service error occurred, please contact support.",
                    serviceException);

            var expectedTppCoordinationServiceException =
                new TppLandingCoordinationServiceException(
                    message: "TPP landing coordination service error occurred, please contact support.",
                    failedTppCoordinationServiceException);

            this.tppLandingOrchestrationServiceMock.Setup(service =>
                service.ProcessAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<Guid>()))
                    .Throws(serviceException);

            // when
            ValueTask<Guid> processTask = this.tppLandingCoordinationService
                .ProcessAsync(input: inputStream, fileName: inputFileName, supplierId: inputSupplierId);

            TppLandingCoordinationServiceException actualException =
                await Assert.ThrowsAsync<TppLandingCoordinationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTppCoordinationServiceException);

            this.tppLandingOrchestrationServiceMock.Verify(service =>
                service.ProcessAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTppCoordinationServiceException))),
                        Times.Once);

            this.tppLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
