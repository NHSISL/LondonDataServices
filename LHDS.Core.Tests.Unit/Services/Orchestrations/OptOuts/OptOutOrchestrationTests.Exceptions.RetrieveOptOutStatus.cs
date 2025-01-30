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
        public async Task ShouldThrowAggregateDependencyValidationExceptionOnRetrieveStatusIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            List<Exception> exceptions = new List<Exception>();
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            Dictionary<string, int> fieldMappings = null;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            var randomString = GetRandomString();
            var inputString = randomString;
            var inputBytes = Encoding.UTF8.GetBytes(inputString);
            Stream inputStream = new MemoryStream(inputBytes);
            var randomRecieveName = $"{GetRandomString()}.csv";
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset expireDate = randomDateTimeOffset.AddDays(-optOutConfiguration.ExpiredAfterDays);
            List<OptOutIdentifier> randomOptOuts = CreateRandomOptOutIdentifiersList();
            List<OptOutIdentifier> outputOptOuts = randomOptOuts;

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings))
                    .ReturnsAsync(outputOptOuts);

            foreach (var optOut in outputOptOuts)
            {
                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffsetAsync())
                        .ThrowsAsync(dependencyValidationException);

                var optOutOrchestrationDependencyValidationException =
                    new OptOutOrchestrationDependencyValidationException(
                        message: "Opt Out orchestration dependency validation errors occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(optOutOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve or add opt out for {exceptions.Count} mapped opt outs",
                    exceptions);

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: failedOptOutOrchestrationServiceException);

            // When
            ValueTask<string> retrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(inputStream, randomRecieveName);

            OptOutOrchestrationServiceException actualOptOutOrchestrationServiceException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(retrieveOptOutStatusTask.AsTask);

            // Then
            actualOptOutOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(outputOptOuts.Count));

            var optOutOrchestrationDependencyValidationLoggingException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    optOutOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(outputOptOuts.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualOptOutOrchestrationServiceException))),
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
        public async Task ShouldThrowAggregateDependencyExceptionOnRetrieveStatusIfErrorsInLoopAndLogItAsync(
           Xeption dependencyException)
        {
            // Given
            List<Exception> exceptions = new List<Exception>();
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            Dictionary<string, int> fieldMappings = null;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            var randomString = GetRandomString();
            var inputString = randomString;
            var inputBytes = Encoding.UTF8.GetBytes(inputString);
            Stream inputStream = new MemoryStream(inputBytes);
            var randomRecieveName = $"{GetRandomString()}.csv";
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset expireDate = randomDateTimeOffset.AddDays(-optOutConfiguration.ExpiredAfterDays);
            List<OptOutIdentifier> randomOptOuts = CreateRandomOptOutIdentifiersList();
            List<OptOutIdentifier> outputOptOuts = randomOptOuts;

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings))
                    .ReturnsAsync(outputOptOuts);

            foreach (var optOut in outputOptOuts)
            {
                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffsetAsync())
                        .ThrowsAsync(dependencyException);

                var optOutOrchestrationDependencyValidationException =
                    new OptOutOrchestrationDependencyException(
                        message: "Opt Out orchestration dependency error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(optOutOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve or add opt out for {exceptions.Count} mapped opt outs",
                    exceptions);

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: failedOptOutOrchestrationServiceException);

            // When
            ValueTask<string> retrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(inputStream, randomRecieveName);

            OptOutOrchestrationServiceException actualOptOutOrchestrationServiceException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(retrieveOptOutStatusTask.AsTask);

            // Then
            actualOptOutOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(outputOptOuts.Count));

            var optOutOrchestrationDependencyLoggingException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    optOutOrchestrationDependencyLoggingException))),
                        Times.Exactly(outputOptOuts.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualOptOutOrchestrationServiceException))),
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
        public async Task ShouldThrowAggregateServiceExceptionOnRetrieveStatusIfErrorsInLoopAndLogItAsync()
        {
            // Given
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            Dictionary<string, int> fieldMappings = null;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            var randomString = GetRandomString();
            var inputString = randomString;
            var inputBytes = Encoding.UTF8.GetBytes(inputString);
            Stream inputStream = new MemoryStream(inputBytes);
            var randomRecieveName = $"{GetRandomString()}.csv";
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset expireDate = randomDateTimeOffset.AddDays(-optOutConfiguration.ExpiredAfterDays);
            List<OptOutIdentifier> randomOptOuts = CreateRandomOptOutIdentifiersList();
            List<OptOutIdentifier> outputOptOuts = randomOptOuts;

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings))
                    .ReturnsAsync(outputOptOuts);

            var innerFailedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: innerFailedOptOutOrchestrationServiceException);

            foreach (var optOut in outputOptOuts)
            {
                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffsetAsync())
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerOptOutOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve or add opt out for {exceptions.Count} mapped opt outs",
                    exceptions);

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: failedOptOutOrchestrationServiceException);

            // When
            ValueTask<string> retrieveOptOutStatusTask =
                this.optOutOrchestrationService.RetrieveOptOutStatusAsync(inputStream, randomRecieveName);

            OptOutOrchestrationServiceException actualOptOutOrchestrationServiceException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(retrieveOptOutStatusTask.AsTask);

            // Then
            actualOptOutOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.csvHelperBrokerMock.Verify(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(outputOptOuts.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    innerOptOutOrchestrationServiceException))),
                        Times.Exactly(outputOptOuts.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutOrchestrationServiceException))),
                        Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OptOutDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRetrieveOptOutStatusIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.UTF8.GetBytes(randomString);
            Stream randomStream = new MemoryStream(randomBytes);
            var randomRecieveName = GetRandomString();

            var expectedDependencyException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            var randomBytes = Encoding.UTF8.GetBytes(randomString);
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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
