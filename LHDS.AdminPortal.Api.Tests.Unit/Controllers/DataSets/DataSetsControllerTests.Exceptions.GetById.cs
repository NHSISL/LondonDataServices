// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var DataSetValidationException =
                new DataSetValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataSetValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedBadRequestObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(someId))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.GetDataSetByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(someId),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<DataSet>(expectedInternalServerErrorObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.GetDataSetByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundDataSetException =
                new NotFoundDataSetException(
                    dataSetId: someId);

            var DataSetValidationException =
                new DataSetValidationException(
                    message: someMessage,
                    innerException: notFoundDataSetException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundDataSetException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedNotFoundObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.GetDataSetByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
