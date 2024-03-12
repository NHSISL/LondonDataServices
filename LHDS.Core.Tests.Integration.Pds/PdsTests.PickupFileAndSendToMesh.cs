// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using Xunit;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        [Fact]
        public async Task ShouldPickupFileAndSendToMeshAsync()
        {
            // Given
            byte[] fileBytes =
                File.ReadAllBytes(
                    @"Resources\MPTREQ_20240310110100.csv");

            FileInfo fi =
                new FileInfo(
                    @"Resources\MPTREQ_20240310110100.csv");

            var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            // When
            PdsAudit pdsAudit =
                await pdsClient.PickupFileAndSendToMesh(pdsFile: fileBytes, fileName: fileName);

            // Then
            pdsAudit.Should().NotBeNull();
            await pdsAuditService.RemovePdsAuditByIdAsync(pdsAudit.Id);
            //var messageId = pdsAudit.MessageId;
            //bool messageAcked = await meshService.AcknowledgeMessageByIdAsync(messageId);
            // messageAcked.Should().BeTrue();
        }
    }
}
