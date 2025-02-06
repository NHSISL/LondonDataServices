// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attrify.Attributes;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SpecificationObjects
{
    public partial class SpecificationObjectsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject addedSpecificationObject = inputSpecificationObject.DeepClone();
            SpecificationObject expectedSpecificationObject = addedSpecificationObject.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedSpecificationObject);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedObjectResult);

            this..Setup(service =>
                service.AddSpecificationObjectAsync(inputSpecificationObject))
                    .ReturnsAsync(addedSpecificationObject);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PostSpecificationObjectAsync(inputSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.AddSpecificationObjectAsync(inputSpecificationObject),
                   Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
