// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberAgreements
{
    public partial class SubscriberAgreementControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            Address randomAddress = CreateRandomAddress();
            Guid inputId = randomAddress.Id;
            Address storageAddress = randomAddress.DeepClone();
            Address expectedAddress = storageAddress.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedAddress);

            var expectedActionResult =
                new ActionResult<Address>(expectedObjectResult);

            this.addressServiceMock.Setup(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageAddress);

            // when
            ActionResult<Address> actualActionResult =
                await this.addressesController.DeleteAddressByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.RemoveAddressByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
