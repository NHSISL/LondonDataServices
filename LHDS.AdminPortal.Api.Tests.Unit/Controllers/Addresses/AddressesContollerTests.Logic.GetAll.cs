// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task ShouldReturnAddressesOnGetAsync()
        {
            // given 
            IQueryable<Address> randomAddresses = CreateRandomAddresses();
            IQueryable<Address> storageAddresses = randomAddresses.DeepClone();
            IQueryable<Address> expectedAddresses = storageAddresses.DeepClone();

            var expectedObjectResult = new OkObjectResult(expectedAddresses);

            var expectedActionResult =
                new ActionResult<IQueryable<Address>>(expectedObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(expectedAddresses);

            // when
            ActionResult<IQueryable<Address>> actualActionResult =
                await this.addressesController.GetAllAddressesAsync();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once());

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
