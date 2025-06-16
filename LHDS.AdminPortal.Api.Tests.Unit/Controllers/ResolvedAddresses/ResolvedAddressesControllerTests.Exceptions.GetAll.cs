// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<ResolvedAddress> someResolvedAddress = CreateRandomResolvedAddresses();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<ResolvedAddress>>(expectedInternalServerErrorObjectResult);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<ResolvedAddress>> actualActionResult =
                await this.resolvedAddressesController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once());

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
        }
    }
}
