// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress addedResolvedAddress = inputResolvedAddress.DeepClone();
            ResolvedAddress expectedResolvedAddress = addedResolvedAddress.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedResolvedAddress);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.AddResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(addedResolvedAddress);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.PostResolvedAddressAsync(inputResolvedAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(inputResolvedAddress),
                   Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
