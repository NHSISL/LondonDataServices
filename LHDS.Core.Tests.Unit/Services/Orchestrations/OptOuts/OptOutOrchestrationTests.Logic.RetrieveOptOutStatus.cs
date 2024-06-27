// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
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
            var inputBytes = Encoding.ASCII.GetBytes(inputString);
            Stream inputStream = new MemoryStream(inputBytes);
            var randomRecieveName = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset expireDate = randomDateTimeOffset.AddDays(-optOutConfiguration.ExpiredAfterDays);
            List<OptOutIdentifier> randomOptOuts = CreateRandomOptOutIdentifiersList();
            List<OptOutIdentifier> outputOptOuts = randomOptOuts;

            this.csvHelperBrokerMock.Setup(processing =>
                processing.MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader, fieldMappings))
                    .ReturnsAsync(outputOptOuts);

            this.identifierBrokerMock.Setup(processing =>
                processing.GetIdentifier())
                    .Returns(identifier);

            List<OptOut> processedOptOuts = new List<OptOut>();

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

                processedOptOuts.Add(storageOptOut);
            }

            var randomOptOutData = GetRandomString();
            var processedString = randomOptOutData;

            this.csvHelperBrokerMock.Setup(processings =>
                processings.MapObjectToCsvAsync(
                    It.Is(SameOptOutListAs(processedOptOuts)),
                    withHeader,
                    fieldMappings,
                    shouldAddTrailingComma))
                        .ReturnsAsync(processedString);

            var processedBytes = Encoding.ASCII.GetBytes(processedString);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            Document document = new Document
            {
                FileName = $"{optOutConfiguration.OutputFolder}/{randomRecieveName}_Response.csv",
                //DocumentData = processedBytes
            };

            //this.documentProcessingServiceMock.Setup(service =>
            //    service.AddDocumentAsync(document, It.IsAny<string>()));

            // when
            await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(inputStream, randomRecieveName);

            // then
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

                var storageOptOut = inputOptOut;

                this.optOutProcessingServiceMock.Verify(service =>
                    service.RetrieveOrAddOptOutAsync(It.Is(SameOptOutAs(inputOptOut))),
                        Times.Exactly(outputOptOuts.Count));

                processedOptOuts.Add(storageOptOut);
            }

            this.identifierBrokerMock.Verify(processing =>
                processing.GetIdentifier(),
                    Times.Exactly(outputOptOuts.Count));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(outputOptOuts.Count));

            this.csvHelperBrokerMock.Verify(processings =>
                processings.MapObjectToCsvAsync(
                    It.IsAny<List<OptOut>>(),
                    withHeader,
                    fieldMappings,
                    shouldAddTrailingComma),
                        Times.Once);

            //this.documentProcessingServiceMock.Verify(service =>
            //    service.AddDocumentAsync(It.Is(SameDocumentAs(document)), It.IsAny<string>()),
            //        Times.Once);

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
