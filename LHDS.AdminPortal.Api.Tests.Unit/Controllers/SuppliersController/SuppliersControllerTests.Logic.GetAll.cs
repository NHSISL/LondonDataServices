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
        public async Task ShouldReturnSuppliersOnGetAsync()
        {
            // given 
            IQueryable<Supplier> randomSupplier = CreateRandomSuppliers();
            IQueryable<Supplier> storageSupplier = randomSupplier.DeepClone();
            IQueryable<Supplier> expectedSupplier = storageSupplier.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedSupplier);

            var expectedActionResult =
                new ActionResult<IQueryable<Supplier>>(expectedObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RetrieveAllSuppliersAsync())
                    .ReturnsAsync(expectedSupplier);

            // when
            ActionResult<IQueryable<Supplier>> actualActionResult =
                await this.suppliersController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RetrieveAllSuppliersAsync(),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}