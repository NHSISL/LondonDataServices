// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Manual.OptOuts
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task RetrieveUpdatedMeshConsentStatusesChanges()
        {
            try
            {
                //OptOut inputOptOut = new OptOut();

                //MeshMessage message = new MeshMessage
                //{
                //    StringContent = processedOutputString,
                //    Headers = new Dictionary<string, List<string>>()
                //};

                //message.Headers.Add("Content-Type", new List<string> { "text/plain" });
                //message.Headers.Add("Mex-FileName", new List<string> { batchReference });
                //message.Headers.Add("Mex-From", new List<string> { this.meshConfiguration.MailboxId });
                //message.Headers.Add("Mex-To", new List<string> { this.optOutConfiguration.To });
                //message.Headers.Add("Mex-WorkflowID", new List<string> { this.optOutConfiguration.WorkflowId });

                //List<MeshMessage> messages = await optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync();
                //message.Should().NotBeNull();
                //message.MessageId.Should().NotBeNullOrWhiteSpace();
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }

        }
    }
}