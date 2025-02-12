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

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = inputDataSetSpecification.DeepClone();
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedDataSetSpecification);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification))
                    .ReturnsAsync(storageDataSetSpecification);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.PutDataSetSpecificationAsync(inputDataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification),
                   Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
