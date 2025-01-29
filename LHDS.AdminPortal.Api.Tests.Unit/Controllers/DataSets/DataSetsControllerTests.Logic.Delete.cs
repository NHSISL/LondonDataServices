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
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            Guid inputId = randomDataSet.Id;
            DataSet storageDataSet = randomDataSet.DeepClone();
            DataSet expectedDataSet = storageDataSet.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedDataSet);

            var expectedActionResult =
                new ActionResult<DataSet>(expectedObjectResult);

            this.dataSetServiceMock.Setup(service =>
                service.RemoveDataSetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageDataSet);

            // when
            ActionResult<DataSet> actualActionResult =
                await this.dataSetsController.DeleteDataSetByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetServiceMock.Verify(service =>
                service.RemoveDataSetByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}
