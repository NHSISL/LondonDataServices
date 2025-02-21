// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
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
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement.DeepClone();
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSubscriberAgreement);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedObjectResult);

            this.addressServiceMock.Setup(service =>
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement))
                    .ReturnsAsync(storageSubscriberAgreement);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.addressesController.PutSubscriberAgreementAsync(inputSubscriberAgreement);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.addressServiceMock.Verify(service =>
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement),
                   Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
        }
    }
}
