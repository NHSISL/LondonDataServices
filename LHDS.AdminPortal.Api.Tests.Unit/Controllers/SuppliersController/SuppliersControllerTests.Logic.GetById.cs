// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
    {
        [Fact]
        public async Task ShouldReturnSupplierOnGetByIdAsync()
        {
            // given 
            Supplier randomSupplier = CreateRandomSupplier();
            Guid inputId = randomSupplier.Id;
            Supplier storageSupplier = randomSupplier.DeepClone();
            Supplier expectedSupplier = storageSupplier.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedSupplier);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RetrieveSupplierByIdAsync(inputId))
                    .ReturnsAsync(expectedSupplier);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.GetSupplierByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RetrieveSupplierByIdAsync(inputId),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}
