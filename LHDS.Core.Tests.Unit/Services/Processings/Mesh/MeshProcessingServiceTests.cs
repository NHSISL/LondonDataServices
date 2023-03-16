// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Processings.Mesh;
using Moq;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        private readonly Mock<IMeshService> meshServiceMock = new Mock<IMeshService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IMeshProcessingService meshProcessingService;

        public MeshProcessingServiceTests()
        {
            this.meshProcessingService = new MeshProcessingService(
                this.meshServiceMock.Object,
                this.loggingBrokerMock.Object);
        }
    }
}
