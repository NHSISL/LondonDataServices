// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Orchestrations.AddressToUprns;
using LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressToUprnDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnMatchAddressToUprnIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();

            this.dateTimeBrokerMock
                .Setup(broker => broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomStartTime);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                .ThrowsAsync(dependencyValidationException);

            var expectedDependencyValidationException =
                new AddressToUprnOrchestrationDependencyValidationException(
                    message: "Address to UPRN orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            // when
            ValueTask matchTask = this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: CreateSingleAddressCsvStream(),
                fileName: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: TestContext.Current.CancellationToken);

            AddressToUprnOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnOrchestrationDependencyValidationException>(
                    matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Theory]
        [MemberData(nameof(AddressToUprnDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMatchAddressToUprnIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();

            this.dateTimeBrokerMock
                .Setup(broker => broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomStartTime);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                .ThrowsAsync(dependencyException);

            var expectedDependencyException =
                new AddressToUprnOrchestrationDependencyException(
                    message: "Address to UPRN orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            // when
            ValueTask matchTask = this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: CreateSingleAddressCsvStream(),
                fileName: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: TestContext.Current.CancellationToken);

            AddressToUprnOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnOrchestrationDependencyException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnMatchAddressToUprnIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();
            var serviceException = new Exception();

            this.dateTimeBrokerMock
                .Setup(broker => broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomStartTime);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                .ThrowsAsync(serviceException);

            var failedAddressToUprnOrchestrationServiceException =
                new FailedAddressToUprnOrchestrationServiceException(
                    message: "Failed address to UPRN orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressToUprnOrchestrationServiceException =
                new AddressToUprnOrchestrationServiceException(
                    message: "Address to UPRN orchestration service error occurred, please contact support.",
                    innerException: failedAddressToUprnOrchestrationServiceException);

            // when
            ValueTask matchTask = this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: CreateSingleAddressCsvStream(),
                fileName: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: TestContext.Current.CancellationToken);

            AddressToUprnOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressToUprnOrchestrationServiceException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnOrchestrationServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnOrchestrationServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnMatchAddressToUprnIfFileLogServiceFailsAndLogItAsync()
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            Guid randomFileLogId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();
            DateTimeOffset randomEndTime = randomStartTime.AddSeconds(1);
            string randomAddress = GetRandomString();

            IAsyncEnumerable<AddressToUprnInputCsv> inputRows =
                CreateAsyncEnumerable(new AddressToUprnInputCsv
                {
                    UnstructuredAddress = randomAddress
                });

            var fileLogServiceException =
                new LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
                    .AddressToUprnFileLogServiceException(
                        message: "Address to UPRN file log service error occurred, please contact support.",
                        innerException: new Xeption());

            this.dateTimeBrokerMock
                .SetupSequence(broker => broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomStartTime)
                .ReturnsAsync(randomEndTime);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                .ReturnsAsync(randomFileLogId);

            this.csvHelperBrokerMock
                .Setup(broker => broker.MapCsvToObjectAsync<AddressToUprnInputCsv>(
                    It.IsAny<Stream>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool?>(),
                    It.IsAny<CancellationToken>()))
                .Returns(inputRows);

            this.assignServiceMock
                .Setup(service => service.MatchAddressAsync(randomAddress))
                .ReturnsAsync(CreateRandomAssignAddress());

            this.csvHelperBrokerMock
                .Setup(broker => broker.MapObjectToCsvAsync<AddressToUprnCsv>(
                    It.IsAny<IAsyncEnumerable<AddressToUprnCsv>>(),
                    It.IsAny<Stream>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool?>(),
                    It.IsAny<CancellationToken>()))
                .Returns(async (
                    IAsyncEnumerable<AddressToUprnCsv> objects,
                    Stream stream,
                    bool addHeader,
                    Dictionary<string, int> mappings,
                    bool? trailingComma,
                    CancellationToken ct) =>
                {
                    await foreach (AddressToUprnCsv _ in objects.WithCancellation(ct)) { }
                });

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    this.blobContainers.AddressToUprn))
                .Returns(ValueTask.CompletedTask);

            this.addressToUprnFileLogServiceMock
                .Setup(service => service.AddAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()))
                .ThrowsAsync(fileLogServiceException);

            var expectedDependencyException =
                new AddressToUprnOrchestrationDependencyException(
                    message: "Address to UPRN orchestration dependency error occurred, fix the errors and try again.",
                    innerException: fileLogServiceException.InnerException as Xeption);

            // when
            ValueTask matchTask = this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: new MemoryStream(System.Text.Encoding.UTF8.GetBytes(
                    $"UnstructuredAddress\r\n\"{randomAddress}\"\r\n")),
                fileName: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: TestContext.Current.CancellationToken);

            AddressToUprnOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnOrchestrationDependencyException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
