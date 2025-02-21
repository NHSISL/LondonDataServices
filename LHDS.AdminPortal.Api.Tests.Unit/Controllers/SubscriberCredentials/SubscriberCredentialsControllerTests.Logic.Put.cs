// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential storageSubscriberCredential = inputSubscriberCredential.DeepClone();
            SubscriberCredential expectedSubscriberCredential = storageSubscriberCredential.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSubscriberCredential);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential, false, true))
                    .ReturnsAsync(storageSubscriberCredential);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.PutSubscriberCredentialAsync(inputSubscriberCredential);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential, false, true),
                   Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
