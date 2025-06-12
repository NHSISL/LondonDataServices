// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Addresses
{
    public partial class AddressesControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
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
                await this.addressesController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once());

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
