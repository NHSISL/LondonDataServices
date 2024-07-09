// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        //[Theory]
        //[MemberData(nameof(AddressCoordinationDependencyValidationExceptions))]
        //public async Task
        //    ShouldThrowAggregateDependencyValidationExceptionOnMatchAddressesIfErrorsInLoopAndLogItAsync(
        //    Xeption dependencyValidationException)
        //{
        //    // Given
        //    string someFilename = CreateRandomFileName();
        //    string addressContainer = this.blobContainers.Addresses;
        //    string errorFolder = this.addressConfiguration.ErrorFolder;
        //    string errorFileName = CreateErrorFileName(someFilename, errorFolder);
        //    byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
        //    Stream someStream = new MemoryStream(randomData);
        //    List<ResolvedAddress> randomAddresses = CreateRandomResolvedAddresses();
        //    List<Exception> exceptions = new List<Exception>();

        //    this.resolvedAddressOrchestrationService.Setup(service =>
        //        service.(someStream, someFilename))
        //            .ReturnsAsync(randomAddresses);

        //    foreach (ResolvedAddress address in randomAddresses)
        //    {
        //        this.addressPersistanceOrchestrationServiceMock.Setup(service =>
        //            service.MatchAndPersistResolvedAddressAsync(address))
        //                .ThrowsAsync(dependencyValidationException);

        //        var addressCoordinationDependencyValidationException =
        //            new AddressCoordinationDependencyValidationException(
        //                message: "Address coordination dependency validation error occurred, please try again.",
        //                innerException: dependencyValidationException.InnerException as Xeption);

        //        exceptions.Add(addressCoordinationDependencyValidationException);
        //    }

        //    this.resolvedAddressOrchestrationServiceMock.Setup(service =>
        //        service.AddDocumentAsync(randomData, errorFileName, addressContainer));

        //    this.resolvedAddressOrchestrationServiceMock.Setup(service =>
        //        service.RemoveDocumentByFileNameAsync(someFilename, addressContainer));

        //    var aggregateException =
        //        new AggregateException(
        //            $"Unable to match address for {exceptions.Count} address files",
        //            exceptions);

        //    var failedAddressCoordinationServiceException =
        //        new FailedAddressCoordinationServiceException(
        //            message: "Failed address coordination service aggregate error occurred, " +
        //                "please contact support.",
        //            innerException: aggregateException);

        //    var expectedAddressCoordinationServiceException =
        //        new AddressCoordinationServiceException(
        //            message: "Address coordination service error occurred, please contact support.",
        //            innerException: failedAddressCoordinationServiceException);

        //    // When
        //    ValueTask matchAddressTask =
        //        this.addressCoordinationService.MatchAddressDataAsync(randomData, someFilename);

        //    AddressCoordinationServiceException actualAddressCoordinationServiceException =
        //        await Assert.ThrowsAsync<AddressCoordinationServiceException>(async () =>
        //            await matchAddressTask);

        //    // Then
        //    actualAddressCoordinationServiceException.Should()
        //        .BeEquivalentTo(expectedAddressCoordinationServiceException);

        //    this.addressOrchestrationServiceMock.Verify(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename),
        //            Times.Once);

        //    foreach (ResolvedAddress address in randomAddresses)
        //    {
        //        this.addressPersistanceOrchestrationServiceMock.Verify(service =>
        //            service.MatchAndPersistResolvedAddressAsync(address),
        //                Times.Once);
        //    }

        //    var addressCoordinationDependencyValidationLoggingException =
        //        new AddressCoordinationDependencyValidationException(
        //            message: "Address coordination dependency validation error occurred, please try again.",
        //            innerException: dependencyValidationException.InnerException as Xeption);

        //    this.resolvedAddressOrchestrationServiceMock.Verify(service =>
        //        service.AddDocumentAsync(randomData, errorFileName, addressContainer),
        //            Times.Once);

        //    this.resolvedAddressOrchestrationServiceMock.Verify(service =>
        //        service.RemoveDocumentByFileNameAsync(someFilename, addressContainer),
        //            Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            addressCoordinationDependencyValidationLoggingException))),
        //                Times.Exactly(randomAddresses.Count));

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            expectedAddressCoordinationServiceException))),
        //                Times.Once);

        //    this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}

        //[Theory]
        //[MemberData(nameof(AddressCoordinationDependencyExceptions))]
        //public async Task
        //    ShouldThrowAggregateDependencyExceptionOnMatchAddressesIfErrorsInLoopAndLogItAsync(
        //    Xeption dependencyException)
        //{
        //    string someFilename = CreateRandomFileName();
        //    string addressContainer = this.blobContainers.Addresses;
        //    string errorFolder = this.addressConfiguration.ErrorFolder;
        //    string errorFileName = CreateErrorFileName(someFilename, errorFolder);
        //    byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
        //    List<ResolvedAddress> randomAddresses = CreateRandomResolvedAddresses();
        //    List<Exception> exceptions = new List<Exception>();

        //    this.addressOrchestrationServiceMock.Setup(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename))
        //            .ReturnsAsync(randomAddresses);

        //    foreach (ResolvedAddress address in randomAddresses)
        //    {
        //        this.addressPersistanceOrchestrationServiceMock.Setup(service =>
        //            service.MatchAndPersistResolvedAddressAsync(address))
        //                .ThrowsAsync(dependencyException);

        //        var addressCoordinationDependencyException =
        //            new AddressCoordinationDependencyException(
        //                message: "Address coordination dependency error occurred, please try again.",
        //                innerException: dependencyException.InnerException as Xeption);

        //        exceptions.Add(addressCoordinationDependencyException);
        //    }

        //    var aggregateException =
        //        new AggregateException(
        //            $"Unable to match address for {exceptions.Count} address files",
        //            exceptions);

        //    var failedAddressCoordinationServiceException =
        //        new FailedAddressCoordinationServiceException(
        //            message: "Failed address coordination service aggregate error occurred, " +
        //                "please contact support.",
        //            innerException: aggregateException);

        //    var expectedAddressCoordinationServiceException =
        //        new AddressCoordinationServiceException(
        //            message: "Address coordination service error occurred, please contact support.",
        //            innerException: failedAddressCoordinationServiceException);

        //    // When
        //    ValueTask matchAddressTask =
        //        this.addressCoordinationService.MatchAddressDataAsync(randomData, someFilename);

        //    AddressCoordinationServiceException actualAddressCoordinationServiceException =
        //        await Assert.ThrowsAsync<AddressCoordinationServiceException>(async () =>
        //            await matchAddressTask);

        //    // Then
        //    actualAddressCoordinationServiceException.Should()
        //        .BeEquivalentTo(expectedAddressCoordinationServiceException);

        //    this.addressOrchestrationServiceMock.Verify(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename),
        //            Times.Once);

        //    foreach (ResolvedAddress address in randomAddresses)
        //    {
        //        this.addressPersistanceOrchestrationServiceMock.Verify(service =>
        //            service.MatchAndPersistResolvedAddressAsync(address),
        //                Times.Once);
        //    }

        //    var addressCoordinationDependencyLoggingException =
        //        new AddressCoordinationDependencyException(
        //            message: "Address coordination dependency error occurred, please try again.",
        //            innerException: dependencyException.InnerException as Xeption);

        //    this.resolvedAddressOrchestrationServiceMock.Verify(service =>
        //        service.AddDocumentAsync(randomData, errorFileName, addressContainer),
        //            Times.Once);

        //    this.resolvedAddressOrchestrationServiceMock.Verify(service =>
        //        service.RemoveDocumentByFileNameAsync(someFilename, addressContainer),
        //            Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            addressCoordinationDependencyLoggingException))),
        //                Times.Exactly(randomAddresses.Count));

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            expectedAddressCoordinationServiceException))),
        //                Times.Once);

        //    this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}

        //[Fact]
        //public async Task ShouldThrowAggregateServiceExceptionOnMatchAddressIfErrorsInLoopAndLogItAsync()
        //{
        //    // Given
        //    string someFilename = CreateRandomFileName();
        //    string addressContainer = this.blobContainers.Addresses;
        //    string errorFolder = this.addressConfiguration.ErrorFolder;
        //    string errorFileName = CreateErrorFileName(someFilename, errorFolder);
        //    byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
        //    var serviceException = new Exception();
        //    List<ResolvedAddress> randomAddresses = CreateRandomResolvedAddresses();
        //    List<Exception> exceptions = new List<Exception>();

        //    this.addressOrchestrationServiceMock.Setup(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename))
        //            .ReturnsAsync(randomAddresses);

        //    var innerFailedAddressCoordinationServiceException =
        //        new FailedAddressCoordinationServiceException(
        //            message: "Failed address coordination service error occurred, please contact support.",
        //            innerException: serviceException);

        //    var innerAddressCoordinationServiceException =
        //        new AddressCoordinationServiceException(
        //            message: "Address coordination service error occurred, please contact support.",
        //            innerException: innerFailedAddressCoordinationServiceException);

        //    foreach (ResolvedAddress address in randomAddresses)
        //    {
        //        this.addressPersistanceOrchestrationServiceMock.Setup(service =>
        //            service.MatchAndPersistResolvedAddressAsync(address))
        //                .ThrowsAsync(serviceException);

        //        exceptions.Add(innerAddressCoordinationServiceException);
        //    }

        //    var aggregateException =
        //        new AggregateException(
        //            $"Unable to match address for {exceptions.Count} address files",
        //            exceptions);

        //    var failedAddressCoordinationServiceException =
        //        new FailedAddressCoordinationServiceException(
        //            message: "Failed address coordination service aggregate error occurred, " +
        //                "please contact support.",
        //            innerException: aggregateException);

        //    var expectedAddressCoordinationServiceException =
        //        new AddressCoordinationServiceException(
        //            message: "Address coordination service error occurred, please contact support.",
        //            innerException: failedAddressCoordinationServiceException);

        //    // When
        //    ValueTask matchAddressTask =
        //        this.addressCoordinationService.MatchAddressDataAsync(randomData, someFilename);

        //    AddressCoordinationServiceException actualAddressCoordinationServiceException =
        //        await Assert.ThrowsAsync<AddressCoordinationServiceException>(async () =>
        //            await matchAddressTask);

        //    // Then
        //    actualAddressCoordinationServiceException.Should()
        //        .BeEquivalentTo(expectedAddressCoordinationServiceException);

        //    this.addressOrchestrationServiceMock.Verify(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename),
        //            Times.Once);

        //    foreach (ResolvedAddress address in randomAddresses)
        //    {
        //        this.addressPersistanceOrchestrationServiceMock.Verify(service =>
        //            service.MatchAndPersistResolvedAddressAsync(address),
        //                Times.Once);
        //    }

        //    this.resolvedAddressOrchestrationServiceMock.Verify(service =>
        //        service.AddDocumentAsync(randomData, errorFileName, addressContainer),
        //            Times.Once);

        //    this.resolvedAddressOrchestrationServiceMock.Verify(service =>
        //        service.RemoveDocumentByFileNameAsync(someFilename, addressContainer),
        //            Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            innerAddressCoordinationServiceException))),
        //                Times.Exactly(randomAddresses.Count));

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            expectedAddressCoordinationServiceException))),
        //                Times.Once);

        //    this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}

        //[Theory]
        //[MemberData(nameof(AddressCoordinationDependencyValidationExceptions))]
        //public async Task ShouldThrowDependencyValidationOnMatchAddressDataIfDependencyValidationOccursAndLogItAsync(
        //    Xeption dependancyValidationException)
        //{
        //    // given
        //    string someFilename = GetRandomString();
        //    byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());

        //    var expectedDependencyException =
        //        new AddressCoordinationDependencyValidationException(
        //            message: "Address coordination dependency validation error occurred, please try again.",
        //            innerException: dependancyValidationException.InnerException as Xeption);

        //    this.addressOrchestrationServiceMock.Setup(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename))
        //            .ThrowsAsync(dependancyValidationException);

        //    // when
        //    ValueTask matchAddressDataTask =
        //        this.addressCoordinationService.MatchAddressDataAsync(randomData, someFilename);

        //    AddressCoordinationDependencyValidationException actualException =
        //        await Assert.ThrowsAsync<AddressCoordinationDependencyValidationException>(matchAddressDataTask.AsTask);

        //    // then
        //    actualException.Should()
        //         .BeEquivalentTo(expectedDependencyException);

        //    this.addressOrchestrationServiceMock.Verify(service =>
        //     service.ProcessResolvedAddressesAsync(randomData, someFilename),
        //         Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //       broker.LogError(It.Is(SameExceptionAs(
        //           expectedDependencyException))),
        //               Times.Once);

        //    this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //    this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        //}

        //[Theory]
        //[MemberData(nameof(AddressCoordinationDependencyExceptions))]
        //public async Task ShouldThrowDependencyExceptionOnMatchAddressDataIfDependencyErrorOccursAndLogItAsync(
        //    Xeption dependencyException)
        //{
        //    // given
        //    string someFilename = GetRandomString();
        //    byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());

        //    var expectedDependencyException =
        //        new AddressCoordinationDependencyException(
        //            message: "Address coordination dependency error occurred, please try again.",
        //            innerException: dependencyException.InnerException as Xeption);

        //    this.addressOrchestrationServiceMock.Setup(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename))
        //            .ThrowsAsync(dependencyException);

        //    // when
        //    ValueTask matchAddressDataTask =
        //        this.addressCoordinationService.MatchAddressDataAsync(randomData, someFilename);

        //    AddressCoordinationDependencyException actualException =
        //        await Assert.ThrowsAsync<AddressCoordinationDependencyException>(matchAddressDataTask.AsTask);

        //    // then
        //    actualException.Should()
        //         .BeEquivalentTo(expectedDependencyException);

        //    this.addressOrchestrationServiceMock.Verify(service =>
        //     service.ProcessResolvedAddressesAsync(randomData, someFilename),
        //         Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //       broker.LogError(It.Is(SameExceptionAs(
        //           expectedDependencyException))),
        //               Times.Once);

        //    this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //    this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        //}

        //[Fact]
        //public async Task ShouldThrowServiceExceptionOnMatchAddressDataIfServiceErrorOccursAndLogItAsync()
        //{
        //    // given
        //    string someFilename = GetRandomString();
        //    byte[] randomData = Encoding.UTF8.GetBytes(GetRandomString());
        //    var serviceException = new Exception();

        //    var failedAddressCoordinationServiceException =
        //        new FailedAddressCoordinationServiceException(
        //            message: "Failed address coordination service error occurred, please contact support.",
        //            innerException: serviceException);

        //    var expectedAddressCoordinationServiceException =
        //        new AddressCoordinationServiceException(
        //            message: "Address coordination service error occurred, please contact support.",
        //            innerException: failedAddressCoordinationServiceException);

        //    this.addressOrchestrationServiceMock.Setup(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename))
        //            .ThrowsAsync(serviceException);

        //    // when
        //    ValueTask matchAddressDataTask = this.addressCoordinationService
        //        .MatchAddressDataAsync(randomData, someFilename);

        //    AddressCoordinationServiceException actualException =
        //        await Assert.ThrowsAsync<AddressCoordinationServiceException>(matchAddressDataTask.AsTask);

        //    // then
        //    actualException.Should().BeEquivalentTo(expectedAddressCoordinationServiceException);

        //    this.addressOrchestrationServiceMock.Verify(service =>
        //        service.ProcessResolvedAddressesAsync(randomData, someFilename),
        //            Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            expectedAddressCoordinationServiceException))),
        //                Times.Once);

        //    this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //    this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
        //    this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
        //}
    }
}