// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            Supplier inputSupplier = randomSupplier;
            Supplier storageSupplier = inputSupplier.DeepClone();
            Supplier expectedSupplier = storageSupplier.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSupplier);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.ModifySupplierAsync(inputSupplier))
                    .ReturnsAsync(storageSupplier);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.PutSupplierAsync(inputSupplier);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.ModifySupplierAsync(inputSupplier),
                   Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}
