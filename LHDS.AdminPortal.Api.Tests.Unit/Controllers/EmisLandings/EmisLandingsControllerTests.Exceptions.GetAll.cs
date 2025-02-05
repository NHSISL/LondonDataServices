// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.EmisLandings
{
    public partial class EmisLandingsControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            Guid someSubscriberAgreementId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<List<string>>(expectedInternalServerErrorObjectResult);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync(someSubscriberAgreementId))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<List<string>> actualActionResult =
                await this.emisLandingsController.Get(someSubscriberAgreementId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(someSubscriberAgreementId),
                    Times.Once());

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
