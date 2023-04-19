// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
            bool withHeader = false;
            bool shouldAddTrailingComma = true;
            var randomString = GetRandomString();
            var inputString = randomString;
            var randomBytes = Encoding.ASCII.GetBytes(inputString);
            var inputBytes = randomBytes;
            var randomRecieveName = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<OptOut> randomOptOuts = CreateRandomOptOuts();
            List<OptOut> outputOptOuts = randomOptOuts;

            this.csvMapperProcessingServiceMock.Setup(processing =>
                 processing.MapCsvToObjectAsync<OptOut>(It.Is(SameStringAs(inputString)), withHeader))
                     .ReturnsAsync(outputOptOuts);

            List<OptOut> processedOptOuts = new List<OptOut>();

            foreach (var optOut in outputOptOuts)
            {
                var storageOptOut = optOut;

                this.optOutProcessingServiceMock.Setup(service =>
                   service.RetrieveOrAddOptOutAsync(optOut))
                       .ReturnsAsync(storageOptOut);

                processedOptOuts.Add(storageOptOut);
            }

            var processedString = GetRandomString();
            var processedBytes = Encoding.ASCII.GetBytes(processedString);

            this.csvMapperProcessingServiceMock.Setup(processings =>
                processings.MapObjectToCsvAsync(
                    It.Is(SameOptOutListAs(processedOptOuts)),
                    withHeader,
                    shouldAddTrailingComma))
                        .ReturnsAsync(processedString);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            Document document = new Document
            {
                FileName = $"receive/{randomRecieveName}_Response_{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
                DocumentData = processedBytes
            };

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(document));

            // when
            await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(inputBytes, randomRecieveName);

            // then
            this.csvMapperProcessingServiceMock.Verify(processing =>
                  processing.MapCsvToObjectAsync<OptOut>(inputString, withHeader),
                        Times.Once);

            foreach (var optOut in outputOptOuts)
            {
                var storageOptOut = optOut;

                this.optOutProcessingServiceMock.Verify(service =>
                   service.RetrieveOrAddOptOutAsync(optOut),
                        Times.Once);

                processedOptOuts.Add(storageOptOut);
            }

            this.csvMapperProcessingServiceMock.Verify(processings =>
                processings.MapObjectToCsvAsync(It.IsAny<List<OptOut>>(), withHeader, shouldAddTrailingComma),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
