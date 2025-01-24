// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests : RESTFulController
    {
        private readonly Mock<IDataSetSpecificationService> dataSetSpecifcationServiceMock;
        private readonly DataSetSpecificationsController dataSetSpecifictionController;

        public DataSetSpecificationsControllerTests()
        {
            this.dataSetSpecifcationServiceMock = new Mock<IDataSetSpecificationService>();

            this.dataSetSpecifictionController = new DataSetSpecificationsController(
                this.dataSetSpecifcationServiceMock.Object);
        }
    }
}
