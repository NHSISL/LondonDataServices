// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task ShouldReturnSubscriberPracticesOnGetAsync()
        {
            // given 
            IQueryable<SubscriberPractice> randomSubscriberPractices = CreateRandomSubscriberPractices();
            IQueryable<SubscriberPractice> storageSubscriberPractices = randomSubscriberPractices.DeepClone();
            IQueryable<SubscriberPractice> expectedSubscriberPractices = storageSubscriberPractices.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedSubscriberPractices);

            var expectedActionResult =
                new ActionResult<IQueryable<SubscriberPractice>>(expectedObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberPracticesAsync())
                    .ReturnsAsync(expectedSubscriberPractices);

            // when
            ActionResult<IQueryable<SubscriberPractice>> actualActionResult =
                await this.subscriberAgreementsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberPracticesAsync(),
                    Times.Once());

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }
    }
}
