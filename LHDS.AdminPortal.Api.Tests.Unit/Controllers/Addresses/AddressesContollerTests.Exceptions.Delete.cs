// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Addresses
{
    public partial class AddressesControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<Address>(expectedBadRequestObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.DeleteAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundAddressException =
                new NotFoundAddressException(
                    addressId: someId);

            var addressValidationException =
                new AddressValidationException(
                    message: someMessage,
                    innerException: notFoundAddressException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundAddressException);

            var expectedActionResult =
                new ActionResult<Address>(expectedNotFoundObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.DeleteAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedAddressAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedAddressException =
                new LockedAddressException(
                    message: someMessage,
                    innerException: someInnerException);

            var addressDependencyValidationException =
                new AddressDependencyValidationException(
                    message: someMessage,
                    innerException: lockedAddressException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(addressDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<Address>(expectedBadRequestObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressDependencyValidationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.DeleteAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnDeleteIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<Address>(expectedBadRequestObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.DeleteAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
