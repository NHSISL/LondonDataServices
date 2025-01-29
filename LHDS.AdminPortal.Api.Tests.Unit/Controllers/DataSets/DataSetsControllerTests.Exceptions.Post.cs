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
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
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
                service.AddDataSetAsync(It.IsAny<DataSet>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PostDataSetAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.AddDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedBadRequestObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.AddDataSetAsync(It.IsAny<DataSet>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PostDataSetAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.AddDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
