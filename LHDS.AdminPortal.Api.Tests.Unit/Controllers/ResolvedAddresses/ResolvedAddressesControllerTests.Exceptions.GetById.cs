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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var ResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(ResolvedAddressValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedBadRequestObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(someId))
                    .ThrowsAsync(ResolvedAddressValidationException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.GetResolvedAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(someId),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetByIdIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<ResolvedAddress>(expectedInternalServerErrorObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<ResolvedAddress> actualActionResult =
                await this.resolvedAddressesController.GetResolvedAddressByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
