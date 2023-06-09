// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO.Enumeration;
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
            string fileName = GetRandomString();
            List<MeshMessage> messages = GetRandomMessages(fileName);


            var expectedList = new List<PdsAudit>();

            foreach(var message in messages)
            {
                pdsAudit
            }

            this.pdsOrchestration.Setup(service => 
                service.RetreiveMessagesFromMeshAndUpdateStorage())
                    .ReturnsAsync(expectedList);

            //When
            var actualList = await pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            //Then
            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}
