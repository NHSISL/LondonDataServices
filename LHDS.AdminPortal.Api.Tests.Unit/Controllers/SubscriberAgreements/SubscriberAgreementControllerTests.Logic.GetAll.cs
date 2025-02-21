// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task ShouldReturnSubscriberAgreementsOnGetAsync()
        {
            // given 
            IQueryable<SubscriberAgreement> randomSubscriberAgreements = CreateRandomSubscriberAgreements();
            IQueryable<SubscriberAgreement> storageSubscriberAgreements = randomSubscriberAgreements.DeepClone();
            IQueryable<SubscriberAgreement> expectedSubscriberAgreements = storageSubscriberAgreements.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedSubscriberAgreements);

            var expectedActionResult =
                new ActionResult<IQueryable<SubscriberAgreement>>(expectedObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ReturnsAsync(expectedSubscriberAgreements);

            // when
            ActionResult<IQueryable<SubscriberAgreement>> actualActionResult =
                await this.subscriberAgreementsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once());

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }
    }
}
