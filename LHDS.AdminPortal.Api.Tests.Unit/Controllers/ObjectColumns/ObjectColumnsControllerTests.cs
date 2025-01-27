// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.ObjectColumns;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class ObjectColumnsControllerTests : RESTFulController
    {
        private readonly Mock<IObjectColumnService> objectColumnsServiceMock;
        private readonly ObjectColumnsController objectColumnsController;

        public ObjectColumnsControllerTests()
        {
            this.objectColumnsServiceMock = new Mock<IObjectColumnService>();
            this.objectColumnsController = new ObjectColumnsController(this.objectColumnsServiceMock.Object);
        }
    }
}
