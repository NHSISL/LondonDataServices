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
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            Guid someIngestionTrackingId = Guid.NewGuid();

            var ingestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(ingestionTrackingValidationException.InnerException);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(ingestionTrackingValidationException);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(someIngestionTrackingId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedBadRequestObjectResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId),
                    Times.Once);

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someIngestionTrackingId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundIngestionTrackingException =
                new NotFoundIngestionTrackingException(someIngestionTrackingId);

            var ingestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: someMessage,
                    innerException: notFoundIngestionTrackingException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundIngestionTrackingException);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(ingestionTrackingValidationException);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(someIngestionTrackingId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedNotFoundObjectResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId),
                    Times.Once);

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            Guid someIngestionTrackingId = Guid.NewGuid();

            var invalidReferenceException =
                new InvalidIngestionTrackingReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var ingestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: someMessage,
                    innerException: invalidReferenceException);

            FailedDependencyObjectResult expectedFailedDependencyObjectResult =
                FailedDependency(ingestionTrackingDependencyValidationException.InnerException);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(ingestionTrackingDependencyValidationException);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(someIngestionTrackingId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedFailedDependencyObjectResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId),
                    Times.Once);

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsErrorOccurredAsync()
        {
            // given
            Guid someIngestionTrackingId = Guid.NewGuid();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsException =
                new AlreadyExistsIngestionTrackingException(
                    message: someMessage,
                    innerException: someInnerException);

            var ingestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsException);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(ingestionTrackingDependencyValidationException);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(someIngestionTrackingId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedConflictObjectResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId),
                    Times.Once);

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
