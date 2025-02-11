// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;
using RESTFulSense.Models;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            Supplier someSupplier = CreateRandomSupplier();

            var SupplierValidationException =
                new SupplierValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(SupplierValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedBadRequestObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()))
                    .ThrowsAsync(SupplierValidationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.PutSupplierAsync(someSupplier);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            Supplier someSupplier = CreateRandomSupplier();

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
                service.ModifySupplierAsync(It.IsAny<Supplier>()))
                    .ThrowsAsync(SupplierValidationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.PutSupplierAsync(someSupplier);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidSupplierReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            Supplier someSupplier = CreateRandomSupplier();

            var alreadyExistsSupplierException =
                new InvalidSupplierReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var SupplierDependencyValidationException =
                new SupplierDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSupplierException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(SupplierDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedBadRequestObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()))
                    .ThrowsAsync(SupplierDependencyValidationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.PutSupplierAsync(someSupplier);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsSupplierErrorOccurredAsync()
        {
            // given
            Supplier someSupplier = CreateRandomSupplier();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsSupplierException =
                new AlreadyExistsSupplierException(
                    message: someMessage,
                    innerException: someInnerException);

            var SupplierDependencyValidationException =
                new SupplierDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSupplierException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsSupplierException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedConflictObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()))
                    .ThrowsAsync(SupplierDependencyValidationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.PutSupplierAsync(someSupplier);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            Supplier someSupplier = CreateRandomSupplier();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<Supplier>(expectedBadRequestObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.PutSupplierAsync(someSupplier);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.ModifySupplierAsync(It.IsAny<Supplier>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}
