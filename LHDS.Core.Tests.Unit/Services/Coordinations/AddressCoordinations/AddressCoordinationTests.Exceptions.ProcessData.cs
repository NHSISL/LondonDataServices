// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
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
                    message: "Address coordination dependency validation error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.ProcessData(randomData))
                    .Throws(dependancyValidationException);

            // when
            ValueTask<List<Address>> decryptTask = this.addressCoordinationService.ProcessData(randomData);

            DecryptionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyValidationException>(decryptTask.AsTask);

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