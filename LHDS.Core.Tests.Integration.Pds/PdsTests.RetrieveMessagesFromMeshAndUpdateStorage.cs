// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldRetrieveMessagesFromMeshAndUpdateStorageAsync()
        {
            // Given 
            byte[] fileBytes =
                File.ReadAllBytes(@"Resources\EmisPDSPatientExtract_247BB600-213A-494E-8E90-A4F9190F07DF_20230601T130544.csv");

            string fileName = "RESP_MPTREQ_CCYYMMDDHHMISS_CCYYMMDDHHMISS.csv";

            var meshMessage = await this.meshService.SendMessageAsync(
                   mexTo: this.pdsConfiguration.To,
                   mexWorkflowId: this.pdsConfiguration.WorkflowId,
                   fileContent: fileBytes,
                   mexSubject: string.Empty,
                   mexLocalId: this.identifierBroker.GetIdentifier().ToString(),
                   mexFileName: fileName,
                   mexContentChecksum: string.Empty,
                   contentType: "text/plain",
                   contentEncoding: string.Empty,
                   accept: "application/json");

            // When
            List<PdsAudit> actualPdsAudits =
              await this.pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            // Then
            actualPdsAudits.Should().NotBeNull();
            bool fileNameExists = false;
            Guid messageId = Guid.Empty;

            foreach (var audit in actualPdsAudits)
            {
                if (audit.FileName == fileName)
                {
                    messageId = audit.Id;
                    fileNameExists = true;
                    break;
                }
            }

            fileNameExists.Should().BeTrue();
            await this.meshService.AcknowledgeMessageByIdAsync(meshMessage.MessageId);
            await this.pdsAuditService.RemovePdsAuditByIdAsync(messageId);
        }
    }
}
