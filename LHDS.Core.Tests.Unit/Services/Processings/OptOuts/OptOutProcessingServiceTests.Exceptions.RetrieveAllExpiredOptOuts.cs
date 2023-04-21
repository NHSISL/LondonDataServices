// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllExpiredOptOutsIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            int randomNumber = GetRandomValidExpiryDays(7);

            var expectedOptOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<OptOut>> retrieveAllExpiredOptOutsTask =
                this.optOutProcessingService.RetrieveAllExpiredOptOutsAsync(randomNumber);

            OptOutProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyValidationException>(retrieveAllExpiredOptOutsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyValidationException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}