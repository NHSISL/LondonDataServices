// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Coordinations.EmisLandings;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.EmisLandings
{
    public partial class EmisLandingsControllerTests : RESTFulController
    {
        private readonly Mock<IEmisLandingCoordinationService> emisLandingCoordinationServiceMock;
        private readonly EmisLandingsController emisLandingsController;

        public EmisLandingsControllerTests()
        {
            this.emisLandingCoordinationServiceMock = new Mock<IEmisLandingCoordinationService>();
            this.emisLandingsController = new EmisLandingsController(this.emisLandingCoordinationServiceMock.Object);
        }
    }
}
