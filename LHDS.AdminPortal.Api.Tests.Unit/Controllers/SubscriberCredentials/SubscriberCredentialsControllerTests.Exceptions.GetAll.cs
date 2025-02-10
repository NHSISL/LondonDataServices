// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberCredentials
{
    public partial class SubscriberCredentialsControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<SubscriberCredential> someSubscriberCredentials = CreateRandomSubscriberCredentials();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<SubscriberCredential>>(expectedInternalServerErrorObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllSubscriberCredentialsAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<SubscriberCredential>> actualActionResult =
                await this.subscriberCredentialsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllSubscriberCredentialsAsync(),
                    Times.Once());

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
