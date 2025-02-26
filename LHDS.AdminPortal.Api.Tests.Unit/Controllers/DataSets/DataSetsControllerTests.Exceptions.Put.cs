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
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSet someDataSet = CreateRandomDataSet();

            var DataSetValidationException =
                new DataSetValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataSetValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedBadRequestObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PutDataSetAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            DataSet someDataSet = CreateRandomDataSet();

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
                service.ModifyDataSetAsync(It.IsAny<DataSet>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PutDataSetAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidDataSetReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSet someDataSet = CreateRandomDataSet();

            var alreadyExistsDataSetException =
                new InvalidDataSetReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsDataSetException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(DataSetDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedBadRequestObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()))
                    .ThrowsAsync(DataSetDependencyValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PutDataSetAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsDataSetErrorOccurredAsync()
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsDataSetException =
                new AlreadyExistsDataSetException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsDataSetException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsDataSetException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedConflictObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()))
                    .ThrowsAsync(DataSetDependencyValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PutDataSetAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedBadRequestObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PutDataSetAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
