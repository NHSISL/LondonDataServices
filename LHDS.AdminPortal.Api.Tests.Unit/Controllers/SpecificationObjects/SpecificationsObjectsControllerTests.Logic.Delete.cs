// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SpecificationObjects
{
    public partial class SpecificationObjectsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            Guid inputId = randomSpecificationObject.Id;
            SpecificationObject storageSpecificationObject = randomSpecificationObject.DeepClone();
            SpecificationObject expectedSpecificationObject = storageSpecificationObject.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSpecificationObject);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageSpecificationObject);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.DeleteSpecificationObjectByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
