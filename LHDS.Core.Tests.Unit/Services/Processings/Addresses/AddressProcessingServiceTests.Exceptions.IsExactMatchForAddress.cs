// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Theory(Skip = "No longer used, will refactor out")]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnIsExactMatchIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            //// given
            //string someAddress = GetRandomString();

            //var expectedAddressProcessingDependencyValidationException =
            //    new AddressProcessingDependencyValidationException(
            //        message: "Address processing dependency validation error occurred, please try again.",
            //        innerException: dependencyValidationException.InnerException as Xeption);

            //addressServiceMock.Setup(service =>
            //    service.RetrieveAllAddresses())
            //        .Throws(dependencyValidationException);

            //// when
            //ValueTask<bool> isMatchTask = this.addressProcessingService
            //    .IsExactMatchForAddressAsync(addressToMatch: someAddress);

            //AddressProcessingDependencyValidationException actualException =
            //    await Assert.ThrowsAsync<AddressProcessingDependencyValidationException>(isMatchTask.AsTask);

            //// then
            //actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyValidationException);

            //addressServiceMock.Verify(service =>
            //    service.RetrieveAllAddresses(),
            //        Times.Once);

            //loggingBrokerMock.Verify(broker =>
            //     broker.LogError(It.Is(SameExceptionAs(
            //         expectedAddressProcessingDependencyValidationException))),
            //             Times.Once);

            //addressServiceMock.VerifyNoOtherCalls();
            //loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory(Skip = "No longer used, will refactor out")]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnIsExactMatchIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            //// given
            //string someAddress = GetRandomString();

            //var expectedAddressProcessingDependencyException =
            //    new AddressProcessingDependencyException(
            //        message: "Address processing dependency error occurred, please try again.",
            //        innerException: dependencyException.InnerException as Xeption);

            //addressServiceMock.Setup(service =>
            //    service.RetrieveAllAddresses())
            //        .Throws(dependencyException);

            //// when
            //ValueTask<bool> isMatchTask = this.addressProcessingService
            //    .IsExactMatchForAddressAsync(addressToMatch: someAddress);

            //AddressProcessingDependencyException actualException =
            //    await Assert.ThrowsAsync<AddressProcessingDependencyException>(isMatchTask.AsTask);

            //// then
            //actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyException);

            //addressServiceMock.Verify(service =>
            //    service.RetrieveAllAddresses(),
            //        Times.Once);

            //loggingBrokerMock.Verify(broker =>
            //     broker.LogError(It.Is(SameExceptionAs(
            //         expectedAddressProcessingDependencyException))),
            //             Times.Once);

            //addressServiceMock.VerifyNoOtherCalls();
            //loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "No longer used, will refactor out")]
        public async Task ShouldThrowServiceExceptionOnIsExactMatchIfServiceErrorOccursAsync()
        {
            //// given
            //string someAddress = GetRandomString();

            //var serviceException = new Exception();

            //var failedAddressProcessingServiceException =
            //    new FailedAddressProcessingServiceException(
            //        message: "Failed Address processing service error occurred, please contact support.",
            //        innerException: serviceException);

            //var expectedAddressProcessingServiveException =
            //    new AddressProcessingServiceException(
            //        message: "Address processing service error occurred, please contact support.",
            //        innerException: failedAddressProcessingServiceException);

            //addressServiceMock.Setup(service =>
            //    service.RetrieveAllAddresses())
            //        .Throws(serviceException);

            //// when
            //ValueTask<bool> isMatchTask = this.addressProcessingService
            //    .IsExactMatchForAddressAsync(addressToMatch: someAddress);

            //AddressProcessingServiceException actualException =
            //    await Assert.ThrowsAsync<AddressProcessingServiceException>(isMatchTask.AsTask);

            //// then
            //actualException.Should().BeEquivalentTo(expectedAddressProcessingServiveException);

            //addressServiceMock.Verify(service =>
            //    service.RetrieveAllAddresses(),
            //        Times.Once);

            //loggingBrokerMock.Verify(broker =>
            //     broker.LogError(It.Is(SameExceptionAs(
            //         expectedAddressProcessingServiveException))),
            //             Times.Once);

            //addressServiceMock.VerifyNoOtherCalls();
            //loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
