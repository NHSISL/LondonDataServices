// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject storageSpecificationObject = inputSpecificationObject.DeepClone();
            SpecificationObject expectedSpecificationObject = storageSpecificationObject.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedSpecificationObject);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.ModifySpecificationObjectAsync(inputSpecificationObject))
                    .ReturnsAsync(storageSpecificationObject);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PutSpecificationObjectAsync(inputSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.ModifySpecificationObjectAsync(inputSpecificationObject),
                   Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
