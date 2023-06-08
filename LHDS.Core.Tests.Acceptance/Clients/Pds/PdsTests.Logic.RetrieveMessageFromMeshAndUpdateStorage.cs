// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Services.Orchestrations.Pds;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    public partial class PdsTests
    {
        [Fact]
        public async Task ShouldRetreiveMessagesFromMeshAndUpdateStorageAsync()
        {
            //Given
            List<MeshMessage> messages = GetRandomMessages();

            var expectedList = new List<PdsAudit>();

            this.pdsOrchestrationService.Setup(service => 
                service.RetreiveMessagesFromMeshAndUpdateStorage())
                    .ReturnsAsync(expectedList);

            //When
            var actualList = await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //Then
            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}
