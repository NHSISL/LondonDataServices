// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
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
        public async Task
            ShouldThrowAggregateDependencyValidationExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync(
                Xeption dependencyValidationException)
        {
            // Given
            string randomFileName = GetRandomString();
            string someFileName = randomFileName;
            string inputContent = GetRandomString();
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputContent);
            Stream someStream = new MemoryStream(inputBytes);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ThrowsAsync(dependencyValidationException);

            var expectedResolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation errors occurred, " +
                        "please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            // When
            ValueTask uploadResolvedAddressTask = this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: someStream, fileName: someFileName);

            ResolvedAddressOrchestrationDependencyValidationException
                actualResolvedAddressOrchestrationDependencyValidationException =
                    await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyValidationException>(async () =>
                        await uploadResolvedAddressTask);

            // Then
            actualResolvedAddressOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyValidationException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationDependencyValidationException))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnProcessResolvedAddressesIfErrorsInLoopAndLogItAsync(
                Xeption dependencyException)
        {
            // Given
            string randomFileName = GetRandomString();
            string someFileName = randomFileName;
            string inputContent = GetRandomString();
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputContent);
            Stream someStream = new MemoryStream(inputBytes);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ThrowsAsync(dependencyException);

            var expectedResolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency errors occurred, " +
                        "please contact support.",
                    innerException: dependencyException.InnerException as Xeption);



            // When
            ValueTask uploadResolvedAddressTask = this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: someStream, fileName: someFileName);

            ResolvedAddressOrchestrationDependencyException
                actualResolvedAddressOrchestrationDependencyException =
                    await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyException>(
                        uploadResolvedAddressTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationDependencyException))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnUploadIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();
            string randomFileName = GetRandomString();
            string someFileName = randomFileName;
            string inputContent = GetRandomString();
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputContent);
            Stream someStream = new MemoryStream(inputBytes);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ThrowsAsync(serviceException);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask uploadResolvedAddressTask = this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: someStream, fileName: someFileName);

            ResolvedAddressOrchestrationServiceException
                actualResolvedAddressOrchestrationServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(
                        uploadResolvedAddressTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}
