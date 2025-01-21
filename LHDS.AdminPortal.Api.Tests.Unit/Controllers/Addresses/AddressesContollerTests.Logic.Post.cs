// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Addresses
{
    public partial class AddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address addedAddress = inputAddress.DeepClone();
            Address expectedAddress = addedAddress.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedAddress);

            var expectedActionResult =
                new ActionResult<Address>(expectedObjectResult);

            this.addressServiceMock.Setup(service =>
                service.AddAddressAsync(inputAddress))
                    .ReturnsAsync(addedAddress);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.PostAddressAsync(randomAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(inputAddress),
                   Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
