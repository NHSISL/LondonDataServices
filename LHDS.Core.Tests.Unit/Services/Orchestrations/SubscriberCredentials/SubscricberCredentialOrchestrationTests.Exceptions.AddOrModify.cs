//// ---------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------

//using System;
//using System.Threading.Tasks;
//using FluentAssertions;
//using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
//using LHDS.Core.Models.Processings.SubscriberCredentials;
//using Moq;
//using Xeptions;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
//{
//    public partial class SubscriberCredentialOrchestrationServiceTests
//    {
//        [Theory]
//        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyValidationExceptions))]
//        public async Task
//            ShouldThrowDependencyValidationOnModifyOrAddSubscriberCredentialIfDependencyValidationOccursAndLogItAsync(
//            Xeption dependencyValidationException)
//        {
//            // given
//            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
//            SubscriberCredential subscriberCredential = CreateRandomCreateSubscriberCredential(randomDateTimeOffset);

//            var expectedDependencyException =
//                new SubscriberCredentialOrchestrationDependencyValidationException(
//                    message: "Subscriber credential orchestration dependency validation error occurred, " +
//                        "fix the errors and try again.",
//                    innerException: dependencyValidationException.InnerException as Xeption);

//            this.secureDataProcessingServiceMock.Setup(service =>
//                service.AddOrModifySecureDataAsync(It.IsAny<SubscriberCredential>()))
//                    .ThrowsAsync(dependencyValidationException);

//            // when
//            ValueTask<SubscriberCredential> modifyOrAddSubscriberCredentialTask =
//                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(subscriberCredential);

//            SubscriberCredentialOrchestrationDependencyValidationException actualException =
//                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationDependencyValidationException>(
//                    modifyOrAddSubscriberCredentialTask.AsTask);

//            // then
//            actualException.Should()
//                 .BeEquivalentTo(expectedDependencyException);

//            this.secureDataProcessingServiceMock.Verify(service =>
//             service.AddOrModifySecureDataAsync(It.IsAny<SubscriberCredential>()),
//                 Times.Once);

//            this.loggingBrokerMock.Verify(broker =>
//               broker.LogError(It.Is(SameExceptionAs(
//                   expectedDependencyException))),
//                       Times.Once);

//            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//            this.dateTimeBrokerMock.VerifyNoOtherCalls();
//            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
//        }

//        [Theory]
//        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyExceptions))]
//        public async Task
//            ShouldThrowDependencyExceptionOnModifyOrAddSubscriberCredentialIfDependencyErrorOccursAndLogItAsync(
//            Xeption dependencyException)
//        {
//            // given
//            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
//            SubscriberCredential subscriberCredential = CreateRandomCreateSubscriberCredential(randomDateTimeOffset);

//            var expectedDependencyException =
//                new SubscriberCredentialDependencyOrchestrationException(
//                    message: "Subscriber credential orchestration dependency error occurred, " +
//                        "fix the errors and try again.",
//                    innerException: dependencyException.InnerException as Xeption);

//            this.secureDataProcessingServiceMock.Setup(service =>
//                service.AddOrModifySecureDataAsync(It.IsAny<SubscriberCredential>()))
//                    .ThrowsAsync(dependencyException);

//            // when
//            ValueTask<SubscriberCredential> modifyOrAddSubscriberCredentialTask =
//                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(subscriberCredential);

//            SubscriberCredentialDependencyOrchestrationException actualException =
//                await Assert.ThrowsAsync<SubscriberCredentialDependencyOrchestrationException>(
//                    modifyOrAddSubscriberCredentialTask.AsTask);

//            // then
//            actualException.Should()
//                 .BeEquivalentTo(expectedDependencyException);

//            this.secureDataProcessingServiceMock.Verify(service =>
//             service.AddOrModifySecureDataAsync(It.IsAny<SubscriberCredential>()),
//                 Times.Once);

//            this.loggingBrokerMock.Verify(broker =>
//               broker.LogError(It.Is(SameExceptionAs(
//                   expectedDependencyException))),
//                       Times.Once);

//            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//            this.dateTimeBrokerMock.VerifyNoOtherCalls();
//            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task
//            ShouldThrowServiceExceptionOnModifyOrAddSubscriberCredentialIfServiceErrorOccursAndLogItAsync()
//        {
//            //given
//            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
//            SubscriberCredential subscriberCredential = CreateRandomCreateSubscriberCredential(randomDateTimeOffset);
//            var serviceException = new Exception();

//            var failedSubscriberCredentialOrchestrationServiceException =
//                new FailedSubscriberCredentialOrchestrationServiceException(
//                    message: "Failed subscriber credential orchestration service error occurred, please contact support.",
//                    innerException: serviceException);

//            var expectedDependencyException =
//                new SubscriberCredentialOrchestrationServiceException(
//                    message: "Subscriber credential orchestration service error occurred, " +
//                        "fix the errors and try again.",
//                    innerException: failedSubscriberCredentialOrchestrationServiceException);

//            this.secureDataProcessingServiceMock.Setup(service =>
//                service.AddOrModifySecureDataAsync(It.IsAny<SubscriberCredential>()))
//                    .ThrowsAsync(serviceException);

//            // when
//            ValueTask<SubscriberCredential> modifyOrAddSubscriberCredentialTask =
//                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(subscriberCredential);

//            SubscriberCredentialOrchestrationServiceException actualException =
//                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationServiceException>(
//                    modifyOrAddSubscriberCredentialTask.AsTask);

//            // then
//            actualException.Should()
//                 .BeEquivalentTo(expectedDependencyException);

//            this.secureDataProcessingServiceMock.Verify(service =>
//             service.AddOrModifySecureDataAsync(It.IsAny<SubscriberCredential>()),
//                 Times.Once);

//            this.loggingBrokerMock.Verify(broker =>
//               broker.LogError(It.Is(SameExceptionAs(
//                   expectedDependencyException))),
//                       Times.Once);

//            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//            this.dateTimeBrokerMock.VerifyNoOtherCalls();
//            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
//        }
//    }
//}