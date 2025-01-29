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
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.DeleteDataSetSpecificationByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundDataSetSpecificationException =
                new NotFoundDataSetSpecificationException(
                    dataSetSpecificationId: someId);

            var DataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: notFoundDataSetSpecificationException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundDataSetSpecificationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedNotFoundObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataSetSpecificationValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.DeleteDataSetSpecificationByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedDataSetAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedDataSetException =
                new LockedDataSetSpecificationException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataSetDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: someMessage,
                    innerException: lockedDataSetException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(DataSetDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataSetDependencyValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.DeleteDataSetSpecificationByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.RemoveDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
