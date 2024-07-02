// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var randomContainer = GetRandomString();
            var randomFileName = GetRandomString();
            var randomData = Encoding.UTF8.GetBytes(GetRandomString());

            var expectedResolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask documentAddTask = this.resolvedAddressOrchestrationService
                .AddDocumentAsync(data: randomData, fileName: randomFileName, container: randomContainer);

            ResolvedAddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyValidationException>(
                    documentAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyValidationException);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyValidationException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var randomContainer = GetRandomString();
            var randomFileName = GetRandomString();
            var randomData = Encoding.UTF8.GetBytes(GetRandomString());

            var expectedResolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(dependencyException);

            // when
            ValueTask documentAddTask = this.resolvedAddressOrchestrationService
                .AddDocumentAsync(data: randomData, fileName: randomFileName, container: randomContainer);

            ResolvedAddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyException>(documentAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyException);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            var randomContainer = GetRandomString();
            var randomFileName = GetRandomString();
            var randomData = Encoding.UTF8.GetBytes(GetRandomString());
            var serviceException = new Exception();

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressOrchestrationServiveException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            ValueTask addDocumentTask = this.resolvedAddressOrchestrationService
                .AddDocumentAsync(data: randomData, fileName: randomFileName, container: randomContainer);

            ResolvedAddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(addDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationServiveException);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationServiveException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
