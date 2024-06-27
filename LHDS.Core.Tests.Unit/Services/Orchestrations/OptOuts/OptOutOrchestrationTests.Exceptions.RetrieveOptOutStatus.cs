// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(OptOutDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRetrieveOptOutStatusIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.ASCII.GetBytes(randomString);
            Stream randomStream = new MemoryStream(randomBytes);
            var randomRecieveName = GetRandomString();

            var expectedDependencyException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(It.IsAny<string>(), false, null))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<string> retrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(randomStream, randomRecieveName);

            OptOutOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyValidationException>(
                    retrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.csvHelperBrokerMock.Verify(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(It.IsAny<string>(), false, null),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OptOutDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveOptOutStatusIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.ASCII.GetBytes(randomString);
            Stream randomStream = new MemoryStream(randomBytes);
            var randomRecieveName = GetRandomString();

            var expectedDependencyException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<string> retrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(randomStream, randomRecieveName);

            OptOutOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyException>(retrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.csvHelperBrokerMock.Verify(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.ASCII.GetBytes(randomString);
            Stream randomStream = new MemoryStream(randomBytes);
            var randomRecieveName = GetRandomString();
            var serviceException = new Exception();

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedOptOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: failedOptOutOrchestrationServiceException);

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> retrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(randomStream, randomRecieveName);

            OptOutOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(retrieveOptOutStatusTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedOptOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOrchestrationServiceException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
