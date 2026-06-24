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
        public async Task ShouldReturnResolvedAddressOnGetByIdAsync()
        {
            // given 
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            Guid inputId = randomResolvedAddress.Id;
            ResolvedAddress storageResolvedAddress = randomResolvedAddress.DeepClone();
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedResolvedAddress);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(inputId))
                    .ReturnsAsync(expectedResolvedAddress);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.GetResolvedAddressByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(inputId),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
