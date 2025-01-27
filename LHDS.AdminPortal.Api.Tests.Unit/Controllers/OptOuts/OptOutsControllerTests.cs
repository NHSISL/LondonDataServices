// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Services.Processings.OptOuts;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class OptOutsControllerTests : RESTFulController
    {
        private readonly Mock<IOptOutProcessingService> optOutServiceMock;
        private readonly OptOutsController optOutController;

        public OptOutsControllerTests()
        {
            this.optOutServiceMock = new Mock<IOptOutProcessingService>();
            this.optOutController = new OptOutsController(this.optOutServiceMock.Object);
        }
    }
}
