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
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSetSpecification someDataSetSpecifications = CreateRandomDataSetSpecification();

            var DataSetSpecificationsValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataSetSpecificationsValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetSpecificationsValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.PostDataSetSpecificationAsync(someDataSetSpecifications);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            DataSetSpecification someDataSet = CreateRandomDataSetSpecification();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.PostDataSetSpecificationAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
