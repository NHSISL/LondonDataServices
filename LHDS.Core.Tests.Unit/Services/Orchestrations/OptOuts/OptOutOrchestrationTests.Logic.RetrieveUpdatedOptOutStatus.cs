// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusChangesAsync()
        {
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(GetRandomNumber());
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // Given
            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            //Loop over ouytputMessageIds to retrieve by ID to get full message (MeshService Ret and Ack)
            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                List<OptOutIdentifier> randomOutputIdentifiers = CreateRandomListOfOptOutIdentifiers();
                List<OptOutIdentifier> outputOptOutIdentifierConsentedList = randomOutputIdentifiers;

                // Map message content to object
                this.csvMapperProcessingServiceMock.Setup(processings =>
                    processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader))
                        .ReturnsAsync(outputOptOutIdentifierConsentedList);

                string batchReference = GetRandomString();
                List<OptOutIdentifier> randomOutputIdentifierBatch = CreateRandomListOfOptOutIdentifiers();
                randomOutputIdentifierBatch.AddRange(outputOptOutIdentifierConsentedList);
                List<OptOut> randomOptOutBatch = CreateRandomOptOutsList(randomOutputIdentifierBatch, batchReference);
                List<OptOut> outputOptOutBatch = randomOptOutBatch;

                // Get original batch
                this.optOutProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                        .ReturnsAsync(outputOptOutBatch);

                // Work out consented items based on mesh return items
                List<string> consentedIdentifiers = outputOptOutIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

                List<OptOut> consentedoptOutItems =
                    outputOptOutBatch.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber)).ToList();

                // Work out non consented items based on mesh return items
                List<OptOut> nonConsentedoptOutItems = outputOptOutBatch.Except(consentedoptOutItems).ToList();

                // Work out the changes => update each record and check if the consent is different to before,
                // if different add to diff list
                // * if consented optout.OptOutStatus !== "Opt-In", update the database and add to diff list

                List<OptOut> differences = new List<OptOut>();

                foreach (var item in consentedoptOutItems)
                {
                    if (item.OptOutStatus != "Opt-In")
                    {
                        item.UpdatedDate = randomDateTimeOffset;
                        item.CacheTime = randomDateTimeOffset;
                        item.OptOutStatus = "Opt-In";

                        this.optOutProcessingServiceMock.Setup(processings =>
                            processings.ModifyOptOutAsync(item))
                                .ReturnsAsync(item);

                        differences.Add(item);
                    }
                }

                foreach (var item in nonConsentedoptOutItems)
                {
                    if (item.OptOutStatus != "Opt-Out")
                    {
                        item.UpdatedDate = randomDateTimeOffset;
                        item.CacheTime = randomDateTimeOffset;
                        item.OptOutStatus = "Opt-Out";

                        this.optOutProcessingServiceMock.Setup(processings =>
                            processings.ModifyOptOutAsync(item))
                                .ReturnsAsync(item);

                        differences.Add(item);
                    }
                }

                // write diff list to doc
                List<OptOutIdentifier> differentIdentifiers = differences
                    .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    this.optOutConfiguration.OptOutFileRequireTrailingComma);

                this.csvMapperProcessingServiceMock.Setup(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        differentIdentifiers,
                        this.optOutConfiguration.OptOutFileHasHeader,
                        this.optOutConfiguration.OptOutFileRequireTrailingComma))
                            .ReturnsAsync(csvDifferences);

                Models.Foundations.Documents.Document document = new Models.Foundations.Documents.Document
                {
                    DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                    FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
                        $"{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
                };

                this.documentProcessingServiceMock.Setup(processings =>
                    processings.AddDocumentAsync(document));
            }

            // put all returned items as opt in list
            // work out delta for opt out by pulling original file and doing a diff on the list
            // Combine opt out and opt in list
            // Update Cache for opt in and opt out list
            // Add Document and put in out folder

            // When
            await this.optOutOrchestrationService.RetrieveUpdatedMeshOptOutStatusChangesAsync();

            // Then
            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.RetrieveAndAcknowledgeMessageByIdAsync(It.IsAny<string>()),
                    Times.Once);

            this.csvMapperProcessingServiceMock.Verify(processings =>
                processings.MapCsvToObjectAsync<OptOutIdentifier>(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Once);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllOptOutsByBatchReferenceAsync(It.IsAny<string>()),
                    Times.Once);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.ModifyOptOutAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.csvMapperProcessingServiceMock.Verify(processings =>
                 processings.MapObjectToCsvAsync<OptOutIdentifier>(
                     It.IsAny<List<OptOutIdentifier>>(), It.IsAny<bool>(), It.IsAny<bool>()),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(processings =>
                processings.AddDocumentAsync(It.IsAny<Document>()),
                    Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }


    }
}
