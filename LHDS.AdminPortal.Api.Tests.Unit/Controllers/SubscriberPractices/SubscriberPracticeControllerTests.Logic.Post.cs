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
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice addedSubscriberPractice = inputSubscriberPractice.DeepClone();
            SubscriberPractice expectedSubscriberPractice = addedSubscriberPractice.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedSubscriberPractice);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.AddSubscriberPracticeAsync(inputSubscriberPractice))
                    .ReturnsAsync(addedSubscriberPractice);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.PostSubscriberPracticeAsync(inputSubscriberPractice);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.AddSubscriberPracticeAsync(inputSubscriberPractice),
                   Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }
    }
}
