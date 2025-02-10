// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberCredentials
{
    public partial class SubscriberCredentialsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential addedSubscriberCredential = inputSubscriberCredential.DeepClone();
            SubscriberCredential expectedSubscriberCredential = addedSubscriberCredential.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedSubscriberCredential);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential, false, true))
                    .ReturnsAsync(addedSubscriberCredential);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.PostSubscriberCredentialAsync(inputSubscriberCredential);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential, false, true),
                   Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
