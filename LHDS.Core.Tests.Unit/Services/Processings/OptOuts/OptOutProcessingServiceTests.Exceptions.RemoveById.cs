// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(inputOptOut.Id))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<OptOut> optOutRemoveTask =
                this.optOutProcessingService.RemoveOptOutByIdAsync(inputOptOut.Id);

            OptOutProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyValidationException>(optOutRemoveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(inputOptOut.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyValidationException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnRemoveIfDependencyErrorOccursAndLogItAsync(
             Xeption dependencyException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyException =
                new OptOutProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(inputOptOut.Id))
                    .Throws(dependencyException);

            // when
            ValueTask<OptOut> optOutRemoveTask =
                this.optOutProcessingService.RemoveOptOutByIdAsync(inputOptOut.Id);

            OptOutProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyException>(optOutRemoveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOptOutProcessingDependencyException);

            this.optOutServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(inputOptOut.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
        }
    }
}