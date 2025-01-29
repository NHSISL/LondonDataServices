// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet addedDataSet = inputDataSet.DeepClone();
            DataSet expectedDataSet = addedDataSet.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedDataSet);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.AddDataSetAsync(inputDataSet))
                    .ReturnsAsync(addedDataSet);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.PostDataSetAsync(inputDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.AddDataSetAsync(inputDataSet),
                   Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
