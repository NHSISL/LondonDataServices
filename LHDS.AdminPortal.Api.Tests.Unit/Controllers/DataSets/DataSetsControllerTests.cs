// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.DataSets;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests : RESTFulController
    {
        private readonly Mock<IDataSetService> dataSetServiceMock;
        private readonly DataSetsController dataSetsController;

        public DataSetsControllerTests()
        {
            this.dataSetServiceMock = new Mock<IDataSetService>();
            this.dataSetsController = new DataSetsController(this.dataSetServiceMock.Object);
        }
    }
}
