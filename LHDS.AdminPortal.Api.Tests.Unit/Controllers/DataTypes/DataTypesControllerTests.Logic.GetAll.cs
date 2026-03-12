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
using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class DataTypesControllerTests
    {
        [Fact]
        public async Task ShouldReturnDataTypesOnGetAsync()
        {
            // given 
            IQueryable<DataType> randomDataType = CreateRandomDataTypes();
            IQueryable<DataType> storageDataType = randomDataType.DeepClone();
            IQueryable<DataType> expectedDataType = storageDataType.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedDataType);

            var expectedActionResult =
                new ActionResult<IQueryable<DataType>>(expectedObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RetrieveAllDataTypesAsync())
                    .ReturnsAsync(expectedDataType);

            // when
            ActionResult<IQueryable<DataType>> actualActionResult =
                await this.dataTypesController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RetrieveAllDataTypesAsync(),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
