// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.DeleteDataSetSpecificationByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
