// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.SpecificationObjects;
using Moq;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SpecificationObjects
{
    public partial class SpecificationObjectsControllerTests
    {
        private readonly Mock<ISpecificationObjectService> specificationObjectServiceMock;
        private readonly SpecificationObjectsController specificationObjectsController;

        public SpecificationObjectsControllerTests()
        {
            this.specificationObjectServiceMock = new Mock<ISpecificationObjectService>();
            
            this.specificationObjectsController = new SpecificationObjectsController(
                this.specificationObjectServiceMock.Object);
        }
    }
}