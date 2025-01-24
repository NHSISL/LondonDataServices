// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests : RESTFulController
    {
        private readonly Mock<IIngestionTrackingService> ingestionTrackingServiceMock;
        private readonly IngestionTrackingsController ingestionTrackingsController;

        public IngestionTrackingsControllerTests()
        {
            this.ingestionTrackingServiceMock = new Mock<IIngestionTrackingService>();

            this.ingestionTrackingsController = new IngestionTrackingsController(
                this.ingestionTrackingServiceMock.Object);
        }
    }
}
