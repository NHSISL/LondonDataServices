// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using Xunit;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldRetrieveMessagesFromMeshAndUpdateStorageAsync()
        {
            // Given
            string pdsFileContainer = "pds";
            byte[] fileBytes =
                File.ReadAllBytes(
                    @"Resources\RegistrarPatientRequest_1C75C2B4-026D-4548-9083-7F2A36B88CB4.csv");

            //Add File to blob
            string fileName =
                "1C75C2B4-026D-4548-9083-7F2A36B88CB4/" +
                "RegistrarPatientRequest/" +
                "20250702142718/" +
                "RegistrarPatientRequest_1C75C2B4-026D-4548-9083-7F2A36B88CB4.csv";

            var meshMessage = await this.meshService.SendMessageAsync(
                   mexTo: this.pdsConfiguration.To,
                   mexWorkflowId: this.pdsConfiguration.WorkflowId,
                   content: new MemoryStream(fileBytes),
                   mexSubject: string.Empty,
                   mexLocalId: this.identifierBroker.GetIdentifierAsync().ToString(),
                   mexFileName: fileName,
                   mexContentChecksum: string.Empty,
                   contentType: "text/plain",
                   contentEncoding: string.Empty,
                   accept: "application/json");

            string fileNameReturn =
                $"{this.pdsConfiguration.OutputFolder}/1C75C2B4-026D-4548-9083-7F2A36B88CB4/" +
                "20250702142718/" +
                "RegistrarPatientRequest/" +
                "RegistrarPatientRequest_1C75C2B4-026D-4548-9083-7F2A36B88CB4.csv";

            // When
            List<PdsAudit> actualPdsAudits =
                await this.pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            // Then
            actualPdsAudits.Should().NotBeNull();

            foreach (var audit in actualPdsAudits)
            {
                audit.FileName.Should().BeEquivalentTo(fileNameReturn);
                Stream outputStream = new MemoryStream();

                await this.blobStorageBroker.SelectByFileNameAsync(
                    output: outputStream,
                    fileName: fileNameReturn,
                    container: pdsFileContainer);

                outputStream.Should().NotBeNull();
                await this.blobStorageBroker.DeleteFileAsync(fileName: fileNameReturn, container: pdsFileContainer);
                await this.pdsAuditService.RemovePdsAuditByIdAsync(audit.Id);
            }

            await Task.CompletedTask;
        }
    }
}
