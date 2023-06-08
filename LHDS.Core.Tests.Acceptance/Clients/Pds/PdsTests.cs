// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Clients;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Services.Orchestrations.Pds;
using Moq;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    public partial class PdsTests
    {
        private readonly IPdsClient pdsClient;
        private readonly Mock<IPdsOrchestrationService> pdsOrchestrationServiceMock;
        private readonly IMeshBroker meshBroker;
        private readonly MeshConfiguration meshConfiguration;
        public PdsTests()
        {
            this.pdsOrchestrationServiceMock = new Mock<IPdsOrchestrationService>();
            this.pdsClient = new PdsClient(pdsOrchestrationServiceMock.Object);
            this.meshBroker = new MeshBroker(meshConfiguration);
        }
    }
}
