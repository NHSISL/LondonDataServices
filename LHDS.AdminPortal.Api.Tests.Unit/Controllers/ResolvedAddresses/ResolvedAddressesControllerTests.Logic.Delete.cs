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
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            Guid inputId = randomResolvedAddress.Id;
            ResolvedAddress storageResolvedAddress = randomResolvedAddress.DeepClone();
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedResolvedAddress);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RemoveResolvedAddressByIdAsync(inputId))
                    .ReturnsAsync(storageResolvedAddress);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.DeleteResolvedAddressByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RemoveResolvedAddressByIdAsync(inputId),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
