// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Fact]
        public async Task ShouldReturnDataSetOnGetByIdAsync()
        {
            // given 
            DataSetSpecification randomDataSet = CreateRandomDataSetSpecification();
            Guid inputId = randomDataSet.Id;
            DataSetSpecification storageDataSetSpecification = randomDataSet.DeepClone();
            DataSetSpecification expectedDataSet = storageDataSetSpecification.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedDataSet);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputId))
                    .ReturnsAsync(expectedDataSet);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.GetDataSetSpecificationByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputId),
                    Times.Once());

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
