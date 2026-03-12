// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var DataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataSetSpecificationValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(someId))
                    .ThrowsAsync(DataSetSpecificationValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.GetDataSetSpecificationByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(someId),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<DataSetSpecification>(expectedInternalServerErrorObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.GetDataSetSpecificationByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundDataSetSpecificationException =
                new NotFoundDataSetSpecificationException(
                    dataSetSpecificationId: someId);

            var DataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: notFoundDataSetSpecificationException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundDataSetSpecificationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedNotFoundObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataSetSpecificationValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.GetDataSetSpecificationByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
