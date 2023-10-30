// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessDataIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());

            var expectedDependencyException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation errors occurred, please try again.",
                    innerException: dependancyValidationException);

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.ProcessData(randomData))
                    .Throws(dependancyValidationException);

            // when
            ValueTask<List<Address>> processDataTask = this.addressCoordinationService.ProcessData(randomData);

            AddressCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
             service.ProcessData(randomData),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressCoordinationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessDataIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());

            var expectedDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency errors occurred, please try again.",
                    innerException: dependencyException);

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.ProcessData(randomData))
                    .Throws(dependencyException);

            // when
            ValueTask<List<Address>> processDataTask = this.addressCoordinationService.ProcessData(randomData);

            AddressCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<AddressCoordinationDependencyException>(processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
             service.ProcessData(randomData),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}