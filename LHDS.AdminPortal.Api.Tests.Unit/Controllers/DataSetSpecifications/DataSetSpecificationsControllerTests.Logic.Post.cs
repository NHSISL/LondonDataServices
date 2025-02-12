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
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification addedDataSetSpecification = inputDataSetSpecification.DeepClone();
            DataSetSpecification expectedDataSetSpecification = addedDataSetSpecification.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedDataSetSpecification);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(inputDataSetSpecification))
                    .ReturnsAsync(addedDataSetSpecification);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.PostDataSetSpecificationAsync(inputDataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(inputDataSetSpecification),
                   Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
