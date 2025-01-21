// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
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
                service.AddAddressAsync(It.IsAny<Address>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.PostAddressAsync(someAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(It.IsAny<Address>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            Address someAddress = CreateRandomAddress();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<Address>(expectedBadRequestObjectResult);

            this.addressServiceMock.Setup(service =>
                service.AddAddressAsync(It.IsAny<Address>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.PostAddressAsync(someAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(It.IsAny<Address>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
