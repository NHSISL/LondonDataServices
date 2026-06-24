// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldPickupFileAndSendToMeshAsync()
        {
            // Given
            FileInfo fi =
                new FileInfo(
                    @"Resources\EmisPDSPatientExtract_247BB600-213A-494E-8E90-A4F9190F07DF_20230601T130544.csv");

            var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            // When
            await using FileStream fileStream =
                new FileStream(
                    fi.FullName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 4096,
                    useAsync: true);

            PdsAudit pdsAudit =
                await pdsClient.PickupFileAndSendToMesh(pdsStream: fileStream, fileName: fileName);

            // Then
            pdsAudit.Should().NotBeNull();
            await pdsAuditService.RemovePdsAuditByIdAsync(pdsAudit.Id);
            var messageId = pdsAudit.MessageId;
            bool messageAcked = await meshService.AcknowledgeMessageByIdAsync(messageId);
            messageAcked.Should().BeTrue();
        }
    }
}
