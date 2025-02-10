// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attrify.Attributes;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberCredentials
{
    public partial class SubscriberCredentialsControllerTests
    {
        [Fact]
        public async Task ShouldReturnSubscriberCredentialesOnGetAsync()
        {
            // given 
            IQueryable<SubscriberCredential> randomSubscriberCredentiales = CreateRandomSubscriberCredentiales();
            IQueryable<SubscriberCredential> storageSubscriberCredentiales = randomSubscriberCredentiales.DeepClone();
            IQueryable<SubscriberCredential> expectedSubscriberCredentiales = storageSubscriberCredentiales.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedSubscriberCredentiales);

            var expectedActionResult =
                new ActionResult<IQueryable<SubscriberCredential>>(expectedObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllSubscriberCredentialsAsync())
                    .ReturnsAsync(expectedSubscriberCredentiales);

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
