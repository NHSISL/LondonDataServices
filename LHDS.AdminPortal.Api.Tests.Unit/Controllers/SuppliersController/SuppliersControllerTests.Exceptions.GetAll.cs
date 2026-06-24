// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<Supplier> someSuppliers = CreateRandomSuppliers();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<Supplier>>(expectedInternalServerErrorObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RetrieveAllSuppliersAsync())
                    .ThrowsAsync(serverException);

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
