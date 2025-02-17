// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            string someNhsNumber = GetRandomString();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var OptOutValidationException =
                new OptOutProcessingValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(OptOutValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOptOutByNhsNumberAsync(someNhsNumber))
                    .ThrowsAsync(OptOutValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.GetOptOutByNhsNumberAsync(someNhsNumber);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOptOutByNhsNumberAsync(someNhsNumber),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetByIdIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            string someNhsNumber = GetRandomString();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedInternalServerErrorObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOptOutByNhsNumberAsync(It.IsAny<string>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.GetOptOutByNhsNumberAsync(someNhsNumber);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOptOutByNhsNumberAsync(It.IsAny<string>()),
                    Times.Once());

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
        {
            // given
            string someNhsNumber = GetRandomString();
            string someMessage = GetRandomString();

            var notFoundOptOutException =
                new NotFoundOptOutException(message: $"Couldn't find optOut with optOutId: {someNhsNumber}.");

            var OptOutValidationException =
                new OptOutProcessingValidationException(
                    message: someMessage,
                    innerException: notFoundOptOutException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundOptOutException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedNotFoundObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOptOutByNhsNumberAsync(someNhsNumber))
                    .ThrowsAsync(OptOutValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.GetOptOutByNhsNumberAsync(someNhsNumber);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOptOutByNhsNumberAsync(someNhsNumber),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
