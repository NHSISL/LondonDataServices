// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<Address>(expectedBadRequestObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByIdAsync(someId))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.GetAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(someId),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetByIdIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<Address> someAddresses = CreateRandomAddresses();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<Address>>(expectedInternalServerErrorObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<Address>> actualActionResult =
                await this.addressesController.GetAllAddressesAsync();

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once());

            this.addressServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrieveAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.GetAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
