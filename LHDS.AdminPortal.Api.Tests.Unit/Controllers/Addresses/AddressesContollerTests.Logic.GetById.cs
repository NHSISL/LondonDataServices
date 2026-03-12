// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Addresses
{
    public partial class AddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnAddressOnGetByIdAsync()
        {
            // given 
            Address randomAddress = CreateRandomAddress();
            Guid inputId = randomAddress.Id;
            Address storageAddress = randomAddress.DeepClone();
            Address expectedAddress = storageAddress.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedAddress);

            var expectedActionResult =
                new ActionResult<Address>(expectedObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByIdAsync(inputId))
                    .ReturnsAsync(expectedAddress);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.GetAddressByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(inputId),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
