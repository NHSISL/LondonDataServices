// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            DateTimeOffset randomDateTimeOffset =
               GetRandomDateTimeOffset();

            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll inputTerminologyPoll = someTerminologyPoll.DeepClone();

            var expectedTerminologyPollProcessingDependencyValidationException =
                new TerminologyPollProcessingDependencyValidationException(
                    message: "Terminology poll processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<TerminologyPoll> terminologyAddTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(inputTerminologyPoll);

            TerminologyPollProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyPollProcessingDependencyValidationException>(
                    terminologyAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingDependencyValidationException))),
                         Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
