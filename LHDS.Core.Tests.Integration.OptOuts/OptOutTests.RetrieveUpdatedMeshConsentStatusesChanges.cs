// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using Xunit;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        [Fact(Skip = "Excluded from pipeline")]
        public async Task RetrieveUpdatedMeshConsentStatusesChanges()
        {
            try
            {
                // given
                string batchReference = Guid.NewGuid().ToString();
                await SetupTestNhsNumbersForRetrieveUpdatedMesh(batchReference);
                string content = await SetupSimulatedMeshMessage(batchReference);
                List<OptOutIdentifier> expectedContent = ConvertToOptOutIdentifierList(content)
                    .OrderBy(item => item.NhsNumber).ToList();

                // when
                List<MeshMessage> messages = await this.optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync();

                // then

                foreach (MeshMessage message in messages)
                {
                    string filepath =
                        $"{optOutConfiguration.OutputFolder}/{GetHeaderValue(message, "Mex-LocalID")}"
                        + "_deltaresponse.csv";

                    Document document = await this.documentService.RetrieveDocumentByFileNameAsync(filepath);
                    document.Should().NotBeNull();

                    List<OptOutIdentifier> actualContent =
                        ConvertToOptOutIdentifierList(Encoding.ASCII.GetString(document.DocumentData))
                            .OrderBy(item => item.NhsNumber).ToList();

                    actualContent.Should().BeEquivalentTo(expectedContent);
                    await this.documentService.RemoveDocumentByFileNameAsync(filepath);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }
        }

        private static List<OptOutIdentifier> ConvertToOptOutIdentifierList(string content)
        {
            List<string> stringList = content.Replace("\r\n", string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            List<OptOutIdentifier> optOutIdentifierList = stringList
                .Select(item => new OptOutIdentifier { NhsNumber = item }).ToList();

            return optOutIdentifierList;
        }
    }
}