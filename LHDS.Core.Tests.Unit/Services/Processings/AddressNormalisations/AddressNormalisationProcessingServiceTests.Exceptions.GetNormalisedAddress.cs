// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnGetNormalisedAddressIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;

            var expectedAddressNormalisationProcessingDependencyValidationException =
                new AddressNormalisationProcessingDependencyValidationException(
                    message: "Address normalisation processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressNormalisationServiceMock.Setup(service =>
                service.GetNormalisedAddress(inputAddress))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<AddressNormalisation> getNormalisedAddressTask =
                this.addressNormalisationProcessingService.GetNormalisedAddress(inputAddress);

            AddressNormalisationProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressNormalisationProcessingDependencyValidationException>(
                    getNormalisedAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressNormalisationProcessingDependencyValidationException);

            this.addressNormalisationServiceMock.Verify(service =>
                service.GetNormalisedAddress(inputAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationProcessingDependencyValidationException))),
                         Times.Once);

            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
