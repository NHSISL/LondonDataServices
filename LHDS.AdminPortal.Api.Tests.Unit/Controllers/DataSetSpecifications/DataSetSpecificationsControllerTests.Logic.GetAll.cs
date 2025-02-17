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
        public async Task ShouldReturnDataSetSpecificationsOnGetAsync()
        {
            // given 
            IQueryable<DataSetSpecification> randomDataSet = CreateRandomDataSetSpecifications();
            IQueryable<DataSetSpecification> storageDataSet = randomDataSet.DeepClone();
            IQueryable<DataSetSpecification> expectedDataSet = storageDataSet.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedDataSet);

            var expectedActionResult =
                new ActionResult<IQueryable<DataSetSpecification>>(expectedObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ReturnsAsync(expectedDataSet);

            // when
            ActionResult<IQueryable<DataSetSpecification>> actualActionResult =
                await this.dataSetSpecificationController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once());

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
