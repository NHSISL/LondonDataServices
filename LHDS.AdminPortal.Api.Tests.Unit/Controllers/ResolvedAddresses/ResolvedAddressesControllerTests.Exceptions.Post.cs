// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();

            var ResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(ResolvedAddressValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedBadRequestObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()))
                    .ThrowsAsync(ResolvedAddressValidationException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.PostResolvedAddressAsync(someResolvedAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedBadRequestObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.PostResolvedAddressAsync(someResolvedAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsResolvedAddressErrorOccurredAsync()
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsResolvedAddressException =
                new AlreadyExistsResolvedAddressException(
                    message: someMessage,
                    innerException: someInnerException);

            var ResolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsResolvedAddressException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsResolvedAddressException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedConflictObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()))
                    .ThrowsAsync(ResolvedAddressDependencyValidationException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.PostResolvedAddressAsync(someResolvedAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidResolvedAddressReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();

            var alreadyExistsResolvedAddressException =
                new InvalidResolvedAddressReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var ResolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsResolvedAddressException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(ResolvedAddressDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedBadRequestObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()))
                    .ThrowsAsync(ResolvedAddressDependencyValidationException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.PostResolvedAddressAsync(someResolvedAddress);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
