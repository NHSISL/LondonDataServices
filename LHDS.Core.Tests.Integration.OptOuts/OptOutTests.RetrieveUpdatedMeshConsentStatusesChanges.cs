// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using Xunit;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        [ReleaseCandidateFact]
        public async Task RetrieveUpdatedMeshConsentStatusesChanges()
        {
            try
            {
                // given
                string optOutFileContainer = "optout";
                string batchReference = Guid.NewGuid().ToString();
                var dbItems = await SetupTestNhsNumbersForRetrieveUpdatedMesh(batchReference);
                List<string> idsFromMesh = dbItems.AsQueryable().Take(2).Select(x => x.NhsNumber).ToList();
                string content = await SetupSimulatedMeshMessage(batchReference, idsFromMesh);

                List<OptOutIdentifier> expectedContent = ConvertNhsListToOptOutIdentifierList(content, dbItems)
                    .OrderBy(item => item.NhsNumber).ToList();

                //Stream fileStream = new MemoryStream(fileBytes);
                Stream expectedFileStream = new MemoryStream();

                // when
                List<MeshMessage> messages = await this.optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync();

                // then
                foreach (MeshMessage message in messages)
                {
                    string filepath =
                        $"{optOutConfiguration.OutputFolder}/{GetHeaderValue(message, "mex-localid")}"
                        + "_deltaresponse.csv";

                    await this.documentService
                        .RetrieveDocumentByFileNameAsync(output: expectedFileStream, fileName: filepath, container: optOutFileContainer);

                    expectedFileStream.Should().NotBeNull();
                    StreamReader reader = new StreamReader(expectedFileStream);
                    string fileText = reader.ReadToEnd();

                    List<OptOutIdentifier> actualContent =
                        ConvertToOptOutIdentifierList(fileText, true)
                            .OrderBy(item => item.NhsNumber).ToList();


                    foreach (var actualItem in actualContent)
                    {
                        var expectedItem = expectedContent
                            .FirstOrDefault(item => item.NhsNumber == actualItem.NhsNumber);


                        actualItem.UniqueReference.Should().BeEquivalentTo(expectedItem.UniqueReference);
                        actualItem.Status.Should().BeEquivalentTo(expectedItem.Status);
                    }

                    await this.documentService
                        .RemoveDocumentByFileNameAsync(filename: filepath, container: optOutFileContainer);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }
            await Task.CompletedTask;
        }

        private static List<OptOutIdentifier> ConvertToOptOutIdentifierList(string content, bool hasHeader = false)
        {
            List<OptOutIdentifier> optOutIdentifiers = new List<OptOutIdentifier>();

            string[] lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (hasHeader && line == lines.FirstOrDefault())
                {
                    continue;
                }

                string[] parts = line.Split(',');
                DateTimeOffset? statusChangedDateTime;

                if (!DateTimeOffset.TryParse(GetArrayValue(3, parts), out var parsedDateTime))
                {
                    statusChangedDateTime = null;
                }
                else
                {
                    statusChangedDateTime = parsedDateTime;
                }

                OptOutIdentifier identifier = new OptOutIdentifier
                {
                    UniqueReference = GetArrayValue(0, parts),
                    NhsNumber = GetArrayValue(1, parts),
                    Status = GetArrayValue(2, parts),
                    StatusChangedDateTime = statusChangedDateTime
                };

                optOutIdentifiers.Add(identifier);
            }

            return optOutIdentifiers;
        }

        private static List<OptOutIdentifier> ConvertNhsListToOptOutIdentifierList(string content, List<OptOut> dbItems)
        {
            List<string> optInIdentifiers = content.Replace(",", string.Empty)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();


            List<OptOutIdentifier> consentItems = new List<OptOutIdentifier>();

            foreach (OptOut item in dbItems)
            {
                var consent = new OptOutIdentifier
                {
                    NhsNumber = item.NhsNumber,
                    UniqueReference = item.UniqueReference,
                    Status = optInIdentifiers.Contains(item.NhsNumber) ? "Opt-In" : "Opt-Out"
                };

                consentItems.Add(consent);
            }

            return consentItems;
        }

        private static string GetArrayValue(int position, string[] array)
        {
            if (position > array.Length - 1)
            {
                return string.Empty;
            }

            return array[position];
        }
    }
}