// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldReturnSubscriberPracticeOnGetByIdAsync()
        {
            // given 
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            Guid inputId = randomSubscriberPractice.Id;
            SubscriberPractice storageSubscriberPractice = randomSubscriberPractice.DeepClone();
            SubscriberPractice expectedSubscriberPractice = storageSubscriberPractice.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedSubscriberPractice);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RetrieveSubscriberPracticeByIdAsync(inputId))
                    .ReturnsAsync(expectedSubscriberPractice);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.GetSubscriberPracticeByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RetrieveSubscriberPracticeByIdAsync(inputId),
                    Times.Once());

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }
    }
}
