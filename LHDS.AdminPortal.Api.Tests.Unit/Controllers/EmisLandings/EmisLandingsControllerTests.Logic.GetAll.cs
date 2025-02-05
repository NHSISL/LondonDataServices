// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.EmisLandings
{
    public partial class EmisLandingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnListOfDocumentsOnGetAsync()
        {
            // given
            var subscriberAgreementId = Guid.NewGuid();
            var expectedDocuments = new List<string> { "file1.pdf", "file2.pdf" };

            OkObjectResult expectedObjectResult = new OkObjectResult(expectedDocuments);

            var expectedActionResult =
                new ActionResult<List<string>>(expectedObjectResult);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync(subscriberAgreementId))
                    .ReturnsAsync(expectedDocuments);

            // when
            ActionResult<List<string>> actualActionResult =
                await this.emisLandingsController.Get(subscriberAgreementId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(subscriberAgreementId),
                    Times.Once());

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }

    }
}
