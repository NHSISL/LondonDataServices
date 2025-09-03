// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice storageSubscriberPractice = inputSubscriberPractice.DeepClone();
            SubscriberPractice expectedSubscriberPractice = storageSubscriberPractice.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSubscriberPractice);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.ModifySubscriberPracticeAsync(inputSubscriberPractice))
                    .ReturnsAsync(storageSubscriberPractice);

            // Practice
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.PutSubscriberPracticeAsync(inputSubscriberPractice);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.ModifySubscriberPracticeAsync(inputSubscriberPractice),
                   Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }
    }
}
