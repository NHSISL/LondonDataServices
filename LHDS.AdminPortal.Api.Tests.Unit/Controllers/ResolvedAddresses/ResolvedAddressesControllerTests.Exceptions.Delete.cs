// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------



using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedBadRequestObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RemoveResolvedAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.DeleteResolvedAddressByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RemoveResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundResolvedAddressException =
                new NotFoundResolvedAddressException(
                    resolvedAddressId: someId);

            var ResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: someMessage,
                    innerException: notFoundResolvedAddressException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundResolvedAddressException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedNotFoundObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RemoveResolvedAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(ResolvedAddressValidationException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.DeleteResolvedAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RemoveResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
