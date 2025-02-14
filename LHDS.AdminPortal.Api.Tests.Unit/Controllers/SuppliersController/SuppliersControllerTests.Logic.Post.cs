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
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            Supplier inputSupplier = randomSupplier;
            Supplier addedSupplier = inputSupplier.DeepClone();
            Supplier expectedSupplier = addedSupplier.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedSupplier);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.AddSupplierAsync(inputSupplier))
                    .ReturnsAsync(addedSupplier);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.PostSupplierAsync(inputSupplier);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.AddSupplierAsync(inputSupplier),
                   Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}
