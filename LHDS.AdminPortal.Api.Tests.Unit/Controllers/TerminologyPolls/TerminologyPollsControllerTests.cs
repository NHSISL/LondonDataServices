// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.DataTypes;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class TerminologyPollsControllerTests : RESTFulController
    {
        private readonly Mock<IDataTypeService> dataTypeServiceMock;
        private readonly DataTypesController dataTypesController;

        public TerminologyPollsControllerTests()
        {
            this.dataTypeServiceMock = new Mock<IDataTypeService>();
            this.dataTypesController = new DataTypesController(this.dataTypeServiceMock.Object);
        }
    }
}
