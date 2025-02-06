// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.EmisLandings
{
    public partial class EmisLandingsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someIngestionTrackingId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(validationException);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(someIngestionTrackingId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedBadRequestObjectResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId),
                    Times.Once());

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FailedDependencyExceptions))]
        public async Task ShouldReturnFailedDependencyOnPutIfDependencyErrorOccurredAsync(Xeption dependencyValidationException)
        {
            // given
            Guid someIngestionTrackingId = Guid.NewGuid();

            FailedDependencyObjectResult expectedFailedDependencyObjectResult =
                FailedDependency(dependencyValidationException);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(someIngestionTrackingId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedFailedDependencyObjectResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId),
                    Times.Once());

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }

    }
}
