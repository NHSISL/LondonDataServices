// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task
            ShouldThrowDependencyValidationExceptionOnConsolidateChangesOptOutsIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            int randomNumber = GetRandomNumber();
            string randomString = GetRandomString();
            List<OptOut> randomOptOutsList = CreateRandomOptOutList(randomString);
            List<string> randomStringList = RandomStringList(randomNumber);

            var expectedOptOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<OptOut>> ConsolidateChangesOptOutTask =
                this.optOutProcessingService.ConsolidateOptOutChangesAndReturnChangesOnly(
                    randomOptOutsList,
                    randomStringList);

            OptOutProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyValidationException>(
                    ConsolidateChangesOptOutTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnConsolidateChangesOptOutsIfDependencyErrorOccursAndLogItAsync(
        Xeption dependencyException)
        {
            // given
            int randomNumber = GetRandomNumber();
            string randomString = GetRandomString();
            List<OptOut> randomOptOutsList = CreateRandomOptOutList(randomString);
            List<string> randomStringList = RandomStringList(randomNumber);

            var expectedOptOutProcessingDependencyException =
                new OptOutProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<OptOut>> ConsolidateChangesOptOutTask =
                this.optOutProcessingService.ConsolidateOptOutChangesAndReturnChangesOnly(
                    randomOptOutsList,
                    randomStringList);

            OptOutProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyException>(ConsolidateChangesOptOutTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOptOutProcessingDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyException))),
                            Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnConsolidateChangesOptOutsIfServiceErrorOccursAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            string randomString = GetRandomString();
            List<OptOut> randomOptOutsList = CreateRandomOptOutList(randomString);
            List<string> randomStringList = RandomStringList(randomNumber);

            var serviceException = new Exception();

            var failedOptOutProcessingServiceException =
                new FailedOptOutProcessingServiceException(
                    message: "Failed opt out processing service error occurred, please contact support.",
                    serviceException);

            var expectedOptOutProcessingServiveException =
                new OptOutProcessingServiceException(
                    failedOptOutProcessingServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<OptOut>> ConsolidateChangesOptOutTask =
                this.optOutProcessingService.ConsolidateOptOutChangesAndReturnChangesOnly(
                    randomOptOutsList,
                    randomStringList);

            OptOutProcessingServiceException actualException =
                await Assert.ThrowsAsync<OptOutProcessingServiceException>(ConsolidateChangesOptOutTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOptOutProcessingServiveException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingServiveException))),
                            Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}