// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.Suppliers;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests : RESTFulController
    {
        private readonly Mock<ISupplierService> supplierServiceMock;
        private readonly SuppliersController suppliersController;

        public SuppliersControllerTests()
        {
            this.supplierServiceMock = new Mock<ISupplierService>();
            this.suppliersController = new SuppliersController(this.supplierServiceMock.Object);
        }
    }
}
