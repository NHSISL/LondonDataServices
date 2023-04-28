// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task RetrieveUpdatedMeshConsentStatusesChanges()
        {
            try
            {
                // given
                string batchReference = Guid.NewGuid().ToString();
                await SetupTestNhsNumbersForRetrieveUpdatedMesh(batchReference);
                string content = await SetupSimulatedMeshMessage(batchReference);
                string expectedContent = content;

                // when
                List<MeshMessage> messages = await this.optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync();

                // then

                foreach (MeshMessage message in messages)
                {
                    string filepath =
                        $"{optOutConfiguration.OutputFolder}/{message.Headers["Mex-LocalID"]}_deltaresponse.csv";

                    Document document = await this.documentService.RetrieveDocumentByFileNameAsync(filepath);
                    document.Should().NotBeNull();
                    string actualContent = Encoding.ASCII.GetString(document.DocumentData);
                    actualContent.Should().Be(expectedContent);
                    await this.documentService.RemoveDocumentByFileNameAsync(filepath);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }
        }
    }
}