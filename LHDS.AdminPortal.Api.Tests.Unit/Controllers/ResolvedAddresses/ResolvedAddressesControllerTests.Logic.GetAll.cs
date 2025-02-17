// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnResolvedAddressOnGetAsync()
        {
            // given 
            IQueryable<ResolvedAddress> randomResolvedAddress = CreateRandomResolvedAddresses();
            IQueryable<ResolvedAddress> storageResolvedAddress = randomResolvedAddress.DeepClone();
            IQueryable<ResolvedAddress> expectedResolvedAddress = storageResolvedAddress.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedResolvedAddress);

            var expectedActionResult =
                new ActionResult<IQueryable<ResolvedAddress>>(expectedObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ReturnsAsync(expectedResolvedAddress);

            // when
            ActionResult<IQueryable<ResolvedAddress>> actualActionResult =
                await this.resolvedAddressesController.GetAllResolvedAddressesAsync();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once());

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
