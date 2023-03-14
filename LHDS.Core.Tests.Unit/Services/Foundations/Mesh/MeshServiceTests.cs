// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Services.Foundations.Mesh;
using Moq;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IMeshService meshService;

        public MeshServiceTests()
        {
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.meshService = new MeshService(
                meshBroker: this.meshBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
    }
}
