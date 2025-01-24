// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests : RESTFulController
    {
        private readonly Mock<IIngestionTrackingAuditService> ingestionTrackingAuditServiceMock;
        private readonly IngestionTrackingAuditsController ingestionTrackingAuditsController;

        public IngestionTrackingAuditsControllerTests()
        {
            this.ingestionTrackingAuditServiceMock = new Mock<IIngestionTrackingAuditService>();

            this.ingestionTrackingAuditsController = new IngestionTrackingAuditsController(
                this.ingestionTrackingAuditServiceMock.Object);
        }
    }
}

