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
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Addresses
{
    public partial class AddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            Address someAddress = CreateRandomAddress();

            var addressValidationException =
                new AddressValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(addressValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<Address>(expectedBadRequestObjectResult);

            this.addressServiceMock.Setup(service =>
                service.ModifyAddressAsync(It.IsAny<Address>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.PutAddressAsync(someAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.ModifyAddressAsync(It.IsAny<Address>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            Address someAddress = CreateRandomAddress();

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
                service.ModifyAddressAsync(It.IsAny<Address>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.PutAddressAsync(someAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.ModifyAddressAsync(It.IsAny<Address>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
