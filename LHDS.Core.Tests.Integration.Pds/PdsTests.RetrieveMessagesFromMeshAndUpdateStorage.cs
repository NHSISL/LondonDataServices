// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        //[ReleaseCandidateFact]
        [Fact(Skip = "Conversion to stream")]
        public async Task ShouldRetrieveMessagesFromMeshAndUpdateStorageAsync()
        {
            //// Given
            //string pdsFileContainer = "pds";
            //byte[] fileBytes =
            //    File.ReadAllBytes(
            //        @"Resources\EmisPDSPatientExtract_247BB600-213A-494E-8E90-A4F9190F07DF_20230601T130544.csv");

            ////Add File to blob
            //string fileName = "RESP_MPTREQ_CCYYMMDDHHMISS_CCYYMMDDHHMISS.csv";

            //var meshMessage = await this.meshService.SendMessageAsync(
            //       mexTo: this.pdsConfiguration.To,
            //       mexWorkflowId: this.pdsConfiguration.WorkflowId,
            //       fileContent: fileBytes,
            //       mexSubject: string.Empty,
            //       mexLocalId: this.identifierBroker.GetIdentifier().ToString(),
            //       mexFileName: fileName,
            //       mexContentChecksum: string.Empty,
            //       contentType: "text/plain",
            //       contentEncoding: string.Empty,
            //       accept: "application/json");

            //string fileNameReturn =
            //    $"{this.pdsConfiguration.OutputFolder}/MPTREQ_CCYYMMDDHHMISS_RESP_CCYYMMDDHHMISS.csv";

            //// When
            //List<PdsAudit> actualPdsAudits =
            //    await this.pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //// Then
            //actualPdsAudits.Should().NotBeNull();

            //foreach (var audit in actualPdsAudits)
            //{
            //    audit.FileName.Should().BeEquivalentTo(fileNameReturn);

            //    byte[] checkDocument =
            //            await this.blobStorageBroker.SelectByFileNameAsync(
            //                fileName: fileNameReturn,
            //                container: pdsFileContainer);

            //    checkDocument.Should().NotBeNull();
            //    await this.blobStorageBroker.DeleteFileAsync(fileName: fileNameReturn, container: pdsFileContainer);
            //    await this.pdsAuditService.RemovePdsAuditByIdAsync(audit.Id);
            //}

            //await this.meshService.AcknowledgeMessageByIdAsync(meshMessage.MessageId);
        }
    }
}
