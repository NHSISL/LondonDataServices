// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var SupplierValidationException =
                new SupplierValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(SupplierValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedBadRequestObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RetrieveSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(SupplierValidationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.GetSupplierByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RetrieveSupplierByIdAsync(someId),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetByIdIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedInternalServerErrorObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RetrieveSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.GetSupplierByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RetrieveSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}
