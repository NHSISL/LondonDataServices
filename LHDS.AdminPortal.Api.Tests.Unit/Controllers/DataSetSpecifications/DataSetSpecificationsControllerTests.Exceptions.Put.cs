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
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSetSpecification someDataSet = CreateRandomDataSetSpecification();

            var DataSetValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataSetValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.PutDataSetSpecificationAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
