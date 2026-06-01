// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Orchestrations.AddressToUprns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldMatchAddressToUprnWithSuccessStatusAsync()
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            Guid randomFileLogId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();
            DateTimeOffset randomEndTime = randomStartTime.AddSeconds(1);
            string randomAddress = GetRandomString();
            AssignAddress randomAssignAddress = CreateRandomAssignAddress(matched: true);

            string csvContent =
                $"UnstructuredAddress\r\n" +
                $"\"{randomAddress}\"\r\n";

            Stream inputStream = CreateCsvStream(csvContent);

            IAsyncEnumerable<AddressToUprnInputCsv> inputRows =
                CreateAsyncEnumerable(new AddressToUprnInputCsv
                {
                    UnstructuredAddress = randomAddress
                });

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
                .ReturnsAsync(randomAssignAddress);

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
                .Setup(service => service.AddAddressToUprnFileLogAsync(
                    It.Is<AddressToUprnFileLog>(log =>
                        log.FileName == randomFileName &&
                        log.SuccessStatus == SuccessStatus.Success)))
                .ReturnsAsync(new AddressToUprnFileLog());

            // when
            await this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: inputStream,
                fileName: randomFileName,
                correlationId: randomCorrelationId);

            // then
            this.assignServiceMock.Verify(service =>
                service.MatchAddressAsync(randomAddress),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapCsvToObjectAsync<AddressToUprnInputCsv>(
                    It.IsAny<Stream>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool?>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync<AddressToUprnCsv>(
                    It.IsAny<IAsyncEnumerable<AddressToUprnCsv>>(),
                    It.IsAny<Stream>(),
                    true,
                    null,
                    false,
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    $"{this.addressToUprnConfiguration.OutboxFolder}/{randomFileName}",
                    this.blobContainers.AddressToUprn),
                        Times.Once);

            this.addressToUprnFileLogServiceMock.Verify(service =>
                service.AddAddressToUprnFileLogAsync(
                    It.Is<AddressToUprnFileLog>(log =>
                        log.FileName == randomFileName &&
                        log.SuccessStatus == SuccessStatus.Success)),
                            Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.addressToUprnFileLogServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSetPartialSuccessStatusWhenIndividualAddressFailsAsync()
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            Guid randomFileLogId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();
            DateTimeOffset randomEndTime = randomStartTime.AddSeconds(1);
            string randomAddress = GetRandomString();
            var timeoutException = new Exception("Request timed out.");

            IAsyncEnumerable<AddressToUprnInputCsv> inputRows =
                CreateAsyncEnumerable(new AddressToUprnInputCsv
                {
                    UnstructuredAddress = randomAddress
                });

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
                .ThrowsAsync(timeoutException);

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
                .ReturnsAsync(new AddressToUprnFileLog());

            // when
            await this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: new MemoryStream(System.Text.Encoding.UTF8.GetBytes(
                    $"UnstructuredAddress\r\n\"{randomAddress}\"\r\n")),
                fileName: randomFileName,
                correlationId: randomCorrelationId);

            // then
            this.addressToUprnFileLogServiceMock.Verify(service =>
                service.AddAddressToUprnFileLogAsync(
                    It.Is<AddressToUprnFileLog>(log =>
                        log.SuccessStatus == SuccessStatus.PartialSuccess &&
                        log.ErrorRowCount == 1)),
                            Times.Once);

            this.assignServiceMock.Verify(service =>
                service.MatchAddressAsync(randomAddress),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapCsvToObjectAsync<AddressToUprnInputCsv>(
                    It.IsAny<Stream>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool?>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync<AddressToUprnCsv>(
                    It.IsAny<IAsyncEnumerable<AddressToUprnCsv>>(),
                    It.IsAny<Stream>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool?>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    $"{this.addressToUprnConfiguration.OutboxFolder}/{randomFileName}",
                    this.blobContainers.AddressToUprn),
                        Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    $"{this.addressToUprnConfiguration.ErrorFolder}/{randomFileName}",
                    this.blobContainers.AddressToUprn),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.addressToUprnFileLogServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSetFailedStatusWhenStreamIsNotSeekableAsync()
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            Guid randomFileLogId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();
            DateTimeOffset randomEndTime = randomStartTime.AddSeconds(1);
            Stream nonSeekableStream = new NonSeekableStream();

            this.dateTimeBrokerMock
                .SetupSequence(broker => broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomStartTime)
                .ReturnsAsync(randomEndTime);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                .ReturnsAsync(randomFileLogId);

            this.addressToUprnFileLogServiceMock
                .Setup(service => service.AddAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()))
                .ReturnsAsync(new AddressToUprnFileLog());

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    this.blobContainers.AddressToUprn))
                .Returns(ValueTask.CompletedTask);

            // when
            await this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: nonSeekableStream,
                fileName: randomFileName,
                correlationId: randomCorrelationId);

            // then
            this.addressToUprnFileLogServiceMock.Verify(service =>
                service.AddAddressToUprnFileLogAsync(
                    It.Is<AddressToUprnFileLog>(log =>
                        log.FileName == randomFileName &&
                        log.SuccessStatus == SuccessStatus.Failed)),
                            Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    $"{this.addressToUprnConfiguration.ErrorFolder}/{randomFileName}",
                    this.blobContainers.AddressToUprn),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.addressToUprnFileLogServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSetFailedStatusWhenFileSizeExceedsLimitAsync()
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            Guid randomFileLogId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();
            DateTimeOffset randomEndTime = randomStartTime.AddSeconds(1);

            long oversizedByteCount =
                ((long)this.addressToUprnConfiguration.MaxFileSizeLimitMb * 1024 * 1024) + 1;

            Stream oversizedStream = new OversizedStream(oversizedByteCount);

            this.dateTimeBrokerMock
                .SetupSequence(broker => broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomStartTime)
                .ReturnsAsync(randomEndTime);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                .ReturnsAsync(randomFileLogId);

            this.addressToUprnFileLogServiceMock
                .Setup(service => service.AddAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()))
                .ReturnsAsync(new AddressToUprnFileLog());

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    this.blobContainers.AddressToUprn))
                .Returns(ValueTask.CompletedTask);

            // when
            await this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: oversizedStream,
                fileName: randomFileName,
                correlationId: randomCorrelationId);

            // then
            this.addressToUprnFileLogServiceMock.Verify(service =>
                service.AddAddressToUprnFileLogAsync(
                    It.Is<AddressToUprnFileLog>(log =>
                        log.FileName == randomFileName &&
                        log.SuccessStatus == SuccessStatus.Failed)),
                            Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    $"{this.addressToUprnConfiguration.ErrorFolder}/{randomFileName}",
                    this.blobContainers.AddressToUprn),
                        Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.addressToUprnFileLogServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotWriteErrorFileWhenAllAddressesSucceedAsync()
        {
            // given
            Guid randomCorrelationId = GetRandomGuid();
            Guid randomFileLogId = GetRandomGuid();
            string randomFileName = GetRandomString();
            DateTimeOffset randomStartTime = GetRandomDateTimeOffset();
            DateTimeOffset randomEndTime = randomStartTime.AddSeconds(1);
            string randomAddress = GetRandomString();
            AssignAddress randomAssignAddress = CreateRandomAssignAddress(matched: true);

            IAsyncEnumerable<AddressToUprnInputCsv> inputRows =
                CreateAsyncEnumerable(new AddressToUprnInputCsv
                {
                    UnstructuredAddress = randomAddress
                });

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
                .ReturnsAsync(randomAssignAddress);

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
                    $"{this.addressToUprnConfiguration.OutboxFolder}/{randomFileName}",
                    this.blobContainers.AddressToUprn))
                .Returns(ValueTask.CompletedTask);

            this.addressToUprnFileLogServiceMock
                .Setup(service => service.AddAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()))
                .ReturnsAsync(new AddressToUprnFileLog());

            // when
            await this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: new MemoryStream(System.Text.Encoding.UTF8.GetBytes(
                    $"UnstructuredAddress\r\n\"{randomAddress}\"\r\n")),
                fileName: randomFileName,
                correlationId: randomCorrelationId);

            // then
            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    $"{this.addressToUprnConfiguration.ErrorFolder}/{randomFileName}",
                    It.IsAny<string>()),
                        Times.Never);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    $"{this.addressToUprnConfiguration.OutboxFolder}/{randomFileName}",
                    this.blobContainers.AddressToUprn),
                        Times.Once);
        }

        private static async IAsyncEnumerable<T> CreateAsyncEnumerable<T>(params T[] items)
        {
            foreach (T item in items)
            {
                await Task.Yield();
                yield return item;
            }
        }

        private sealed class NonSeekableStream : Stream
        {
            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException();
            public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
            public override void Flush() { }
            public override int Read(byte[] buffer, int offset, int count) => 0;
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }

        private sealed class OversizedStream : Stream
        {
            private readonly long length;

            public OversizedStream(long length) =>
                this.length = length;

            public override bool CanRead => true;
            public override bool CanSeek => true;
            public override bool CanWrite => false;
            public override long Length => this.length;
            public override long Position { get; set; }
            public override void Flush() { }
            public override int Read(byte[] buffer, int offset, int count) => 0;
            public override long Seek(long offset, SeekOrigin origin) => 0;
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }
    }
}
