// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveOptOutStatusAsync()
        {
            // given
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            Dictionary<string, int> fieldMappings = null;
            Guid identifier = Guid.NewGuid();
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
            string inputContainer = "optout";

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings))
                    .ReturnsAsync(outputOptOuts);

            this.identifierBrokerMock.Setup(processing =>
                processing.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            List<OptOutIdentifier> expectedProcessedOptOutIdentifiers = new List<OptOutIdentifier>();
            List<OptOutIdentifier> actualProcessedOptOutIdentifiers = new List<OptOutIdentifier>();

            foreach (var optOut in outputOptOuts)
            {
                var inputOptOut = new OptOut
                {
                    Id = identifier,
                    NhsNumber = optOut.NhsNumber,
                    Status = string.IsNullOrWhiteSpace(optOut.Status) ? "Unknown" : optOut.Status,
                    UniqueReference = optOut.UniqueReference,
                    CacheTime = expireDate,
                    CreatedDate = randomDateTimeOffset,
                    UpdatedDate = randomDateTimeOffset,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                var storageOptOut = inputOptOut;

                this.optOutProcessingServiceMock.Setup(service =>
                    service.RetrieveOrAddOptOutAsync(It.Is(SameOptOutAs(inputOptOut))))
                        .ReturnsAsync(storageOptOut);

                OptOutIdentifier processedOptOutIdentifier = new OptOutIdentifier
                {
                    NhsNumber = inputOptOut.NhsNumber,
                    UniqueReference = inputOptOut.UniqueReference,
                    Status = inputOptOut.Status,
                    StatusChangedDateTime = inputOptOut.CacheTime
                };

                expectedProcessedOptOutIdentifiers.Add(processedOptOutIdentifier);
            }

            var randomOptOutData = GetRandomString();
            var processedString = randomOptOutData;

            this.csvHelperBrokerMock.Setup(processings =>
                processings.MapObjectToCsvAsync(
                    It.Is(SameOptOutIdentifierListAs(expectedProcessedOptOutIdentifiers)),
                    withHeader,
                    fieldMappings,
                    shouldAddTrailingComma))
                        .ReturnsAsync(processedString);

            var processedBytes = Encoding.UTF8.GetBytes(processedString);
            Stream csvInputStream = new MemoryStream(processedBytes);
            Stream expectedStream = csvInputStream;
            Stream actualStream = new MemoryStream();

            string csvInputFileName = $"{optOutConfiguration.OutputFolder}/" +
                $"{Path.GetFileNameWithoutExtension(randomRecieveName)}_Response.csv";

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.Is(SameStreamAs(csvInputStream)),
                    csvInputFileName,
                    inputContainer))
                .Callback<Stream, string, string>((stream, fileName, container) =>
                {
                    stream.Position = 0;
                    stream.CopyTo(actualStream);
                })
                .Returns(ValueTask.CompletedTask);

            // when
            await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(inputStream, randomRecieveName);

            // then
            Assert.True(IsSameStream(actualStream, expectedStream));

            this.csvHelperBrokerMock.Verify(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings),
                    Times.Once);

            foreach (var optOut in outputOptOuts)
            {
                var inputOptOut = new OptOut
                {
                    Id = identifier,
                    NhsNumber = optOut.NhsNumber,
                    Status = string.IsNullOrWhiteSpace(optOut.Status) ? "Unknown" : optOut.Status,
                    UniqueReference = optOut.UniqueReference,
                    CacheTime = expireDate,
                    CreatedDate = randomDateTimeOffset,
                    UpdatedDate = randomDateTimeOffset,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                OptOutIdentifier processedOptOutIdentifier = new OptOutIdentifier
                {
                    NhsNumber = inputOptOut.NhsNumber,
                    UniqueReference = inputOptOut.UniqueReference,
                    Status = inputOptOut.Status,
                    StatusChangedDateTime = inputOptOut.CacheTime
                };

                this.optOutProcessingServiceMock.Verify(service =>
                    service.RetrieveOrAddOptOutAsync(It.Is(SameOptOutAs(inputOptOut))),
                        Times.Exactly(outputOptOuts.Count));

                actualProcessedOptOutIdentifiers.Add(processedOptOutIdentifier);
            }

            this.identifierBrokerMock.Verify(processing =>
                processing.GetIdentifierAsync(),
                    Times.Exactly(outputOptOuts.Count));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(outputOptOuts.Count));

            this.csvHelperBrokerMock.Verify(processings =>
                processings.MapObjectToCsvAsync(
                    It.Is(SameOptOutIdentifierListAs(actualProcessedOptOutIdentifiers)),
                    withHeader,
                    fieldMappings,
                    shouldAddTrailingComma),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), csvInputFileName, inputContainer),
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
