// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnIsExactMacthIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someAddress = GetRandomString();

            var expectedAddressProcessingDependencyValidationException =
                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            addressServiceMock.Setup(service =>
                service.RetrieveAllAddresses())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<bool> isMatchTask = this.addressProcessingService
                .IsExactMatchForAddressAsync(addressToMacth: someAddress);

            AddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressProcessingDependencyValidationException>(isMatchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyValidationException);

            addressServiceMock.Verify(service =>
                service.RetrieveAllAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressProcessingDependencyValidationException))),
                         Times.Once);

            addressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
