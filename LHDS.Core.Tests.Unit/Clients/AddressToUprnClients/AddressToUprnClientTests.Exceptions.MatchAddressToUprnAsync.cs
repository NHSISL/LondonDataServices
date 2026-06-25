// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Clients.AddressToUprnClient.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Clients.AddressToUprnClients
{
    public partial class AddressToUprnClientTests
    {
        [Theory]
        [MemberData(nameof(AddressToUprnClientDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnMatchAddressToUprnIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Stream randomData = CreateRandomStream();
            string randomFileName = GetRandomString();
            Guid randomCorrelationId = GetRandomGuid();
            CancellationToken cancellationToken = CancellationToken.None;

            var expectedDependencyValidationException = new AddressToUprnClientValidationException(
                message: "Address client validation error occurred, fix errors and try again.",
                innerException: dependencyValidationException.InnerException as Xeption);

            this.addressToUprnOrchestrationServiceMock.Setup(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken))
                .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask matchTask = this.addressToUprnClient.MatchAddressToUprnAsync(
                data: randomData,
                filename: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: cancellationToken);

            AddressToUprnClientValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnClientValidationException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.addressToUprnOrchestrationServiceMock.Verify(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken),
                        Times.Once);

            this.addressToUprnOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressToUprnClientDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMatchAddressToUprnIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Stream randomData = CreateRandomStream();
            string randomFileName = GetRandomString();
            Guid randomCorrelationId = GetRandomGuid();
            CancellationToken cancellationToken = CancellationToken.None;

            var expectedDependencyException = new AddressToUprnClientDependencyException(
                message: "Address client dependency error occurred, please contact support.",
                innerException: dependencyException.InnerException as Xeption);

            this.addressToUprnOrchestrationServiceMock.Setup(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken))
                .ThrowsAsync(dependencyException);

            // when
            ValueTask matchTask = this.addressToUprnClient.MatchAddressToUprnAsync(
                data: randomData,
                filename: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: cancellationToken);

            AddressToUprnClientDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnClientDependencyException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.addressToUprnOrchestrationServiceMock.Verify(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken),
                        Times.Once);

            this.addressToUprnOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressToUprnClientServiceExceptions))]
        public async Task ShouldThrowServiceExceptionOnMatchAddressToUprnIfServiceExceptionOccursAndLogItAsync(
            Xeption serviceException)
        {
            // given
            Stream randomData = CreateRandomStream();
            string randomFileName = GetRandomString();
            Guid randomCorrelationId = GetRandomGuid();
            CancellationToken cancellationToken = CancellationToken.None;

            var expectedServiceException = new AddressToUprnClientServiceException(
                message: "Address client service error occurred, fix errors and try again.",
                innerException: serviceException.InnerException as Xeption);

            this.addressToUprnOrchestrationServiceMock.Setup(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken))
                .ThrowsAsync(serviceException);

            // when
            ValueTask matchTask = this.addressToUprnClient.MatchAddressToUprnAsync(
                data: randomData,
                filename: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: cancellationToken);

            AddressToUprnClientServiceException actualException =
                await Assert.ThrowsAsync<AddressToUprnClientServiceException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedServiceException);

            this.addressToUprnOrchestrationServiceMock.Verify(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken),
                        Times.Once);

            this.addressToUprnOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
