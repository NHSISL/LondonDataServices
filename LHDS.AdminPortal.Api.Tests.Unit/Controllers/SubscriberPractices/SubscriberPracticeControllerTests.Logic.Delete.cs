// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberPractices
{
    public partial class SubscriberPracticeControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            Guid inputId = randomSubscriberPractice.Id;
            SubscriberPractice storageSubscriberPractice = randomSubscriberPractice.DeepClone();
            SubscriberPractice expectedSubscriberPractice = storageSubscriberPractice.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSubscriberPractice);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageSubscriberPractice);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.DeleteSubscriberPracticeByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }
    }
}
