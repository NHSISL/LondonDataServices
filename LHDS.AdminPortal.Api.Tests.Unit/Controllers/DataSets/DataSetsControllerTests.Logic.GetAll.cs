// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LHDS.Core.Models.Foundations.DataSets;
using System.Threading.Tasks;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests
    {
        [Fact]
        public async Task ShouldReturnDataSetsOnGetAsync()
        {
            // given 
            IQueryable<DataSet> randomDataSet = CreateRandomDataSet();
            IQueryable<DataSet> storageDataSet = randomDataSet.DeepClone();
            IQueryable<DataSet> expectedDataSet = storageDataSet.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedDataSet);

            var expectedActionResult =
                new ActionResult<IQueryable<DataSet>>(expectedObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ReturnsAsync(expectedDataSet);

            // when
            ActionResult<IQueryable<DataSet>> actualActionResult =
                await this.dataSetsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once());

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
