// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<DataSetSpecification> someDataSetSpecifications = CreateRandomDataSetSpecifications();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<DataSetSpecification>>(expectedInternalServerErrorObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<DataSetSpecification>> actualActionResult =
                await this.DataSetSpecificationController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once());

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
