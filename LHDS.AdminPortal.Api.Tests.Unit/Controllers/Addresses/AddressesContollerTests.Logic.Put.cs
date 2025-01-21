// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Addresses
{
    public partial class AddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress.DeepClone();
            Address expectedAddress = storageAddress.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedAddress);

            var expectedActionResult =
                new ActionResult<Address>(expectedObjectResult);

            this.addressServiceMock.Setup(service =>
                service.ModifyAddressAsync(inputAddress))
                    .ReturnsAsync(storageAddress);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.PutAddressAsync(inputAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.ModifyAddressAsync(inputAddress),
                   Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
