// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications;
public partial class DataSetSpecificationsControllerTests
{
    [Fact]
    public async Task ShouldReturnOkOnDeleteAsync()
    {
        // given
        DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
        Guid inputId = randomDataSetSpecification.Id;
        DataSetSpecification storageDataSetSpecification = randomDataSetSpecification.DeepClone();
        DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

        var expectedObjectResult =
            new OkObjectResult(expectedDataSetSpecification);

        var expectedActionResult =
            new ActionResult<DataSetSpecification>(expectedObjectResult);

        this.DataSetSpecificationServiceMock.Setup(service =>
            service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(storageDataSetSpecification);

        // when
        ActionResult<DataSetSpecification> actualActionResult =
            await this.DataSetSpecificationController.DeleteDataSetSpecificationByIdAsync(inputId);

        // then
        actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

        this.DataSetSpecificationServiceMock.Verify(service =>
            service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
               Times.Once);

        this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
    }
}
