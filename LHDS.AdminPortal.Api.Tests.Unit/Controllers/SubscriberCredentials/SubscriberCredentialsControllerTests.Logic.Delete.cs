// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberCredentials
{
    public partial class SubscriberCredentialsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            Guid inputId = randomSubscriberCredential.Id;
            SubscriberCredential storageSubscriberCredential = randomSubscriberCredential.DeepClone();
            SubscriberCredential expectedSubscriberCredential = storageSubscriberCredential.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSubscriberCredential);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()));

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.DeleteSubscriberCredentialByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
