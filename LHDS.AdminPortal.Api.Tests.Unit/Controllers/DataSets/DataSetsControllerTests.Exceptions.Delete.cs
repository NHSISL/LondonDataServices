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
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedBadRequestObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.RemoveDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.DeleteDataSetByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RemoveDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
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
                service.RemoveDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.DeleteDataSetByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RemoveDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
