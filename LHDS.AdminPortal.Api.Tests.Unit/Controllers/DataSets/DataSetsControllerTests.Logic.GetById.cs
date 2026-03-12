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
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests
    {
        [Fact]
        public async Task ShouldReturnDataSetOnGetByIdAsync()
        {
            // given 
            DataSet randomDataSet = CreateRandomDataSet();
            Guid inputId = randomDataSet.Id;
            DataSet storageDataSet = randomDataSet.DeepClone();
            DataSet expectedDataSet = storageDataSet.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedDataSet);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(inputId))
                    .ReturnsAsync(expectedDataSet);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.GetDataSetByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(inputId),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
