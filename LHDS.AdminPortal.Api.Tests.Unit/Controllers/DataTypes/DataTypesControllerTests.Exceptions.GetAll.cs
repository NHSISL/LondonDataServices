// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class DataTypesControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<DataType> someDataTypes = CreateRandomDataTypes();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<DataType>>(expectedInternalServerErrorObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RetrieveAllDataTypesAsync())
                    .ThrowsAsync(serverException);

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
