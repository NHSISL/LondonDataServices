// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldReturnSubscriberAgreementOnGetByIdAsync()
        {
            // given 
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            Guid inputId = randomSubscriberAgreement.Id;
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement.DeepClone();
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedSubscriberAgreement);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputId))
                    .ReturnsAsync(expectedSubscriberAgreement);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.GetSubscriberAgreementByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputId),
                    Times.Once());

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }
    }
}
