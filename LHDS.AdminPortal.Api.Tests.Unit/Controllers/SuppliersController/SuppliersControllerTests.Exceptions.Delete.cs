// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests
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
                new ActionResult<Supplier>(expectedBadRequestObjectResult);

            this.supplierServiceMock.Setup(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<Supplier> actualActionResult =
                await this.suppliersController.DeleteSupplierByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.supplierServiceMock.Verify(service =>
                service.RemoveSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.supplierServiceMock.VerifyNoOtherCalls();
        }
    }
}
