// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using RESTFulSense.Models;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedBadRequestObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.DeleteSupplierByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundSupplierException =
                new NotFoundSupplierException(
                    supplierId: someId);

            var SupplierValidationException =
                new SupplierValidationException(
                    message: someMessage,
                    innerException: notFoundSupplierException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundSupplierException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedNotFoundObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(SupplierValidationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.DeleteSupplierByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedSupplierAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedSupplierException =
                new LockedSupplierException(
                    message: someMessage,
                    innerException: someInnerException);

            var SupplierDependencyValidationException =
                new SupplierDependencyValidationException(
                    message: someMessage,
                    innerException: lockedSupplierException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(SupplierDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedBadRequestObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(SupplierDependencyValidationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.DeleteSupplierByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}
