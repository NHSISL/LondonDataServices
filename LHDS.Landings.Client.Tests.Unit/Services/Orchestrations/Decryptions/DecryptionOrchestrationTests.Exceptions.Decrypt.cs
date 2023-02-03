// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions;
using LHDS.Landings.Client.Models.Orchestrations.Decryptions;
using LHDS.Landings.Client.Models.Orchestrations.Downloads.Exceptions;
using Moq;
using Xeptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DecryptionDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnDecryptIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            string randomFileName = GetRandomMessage();
            byte[] randomEncryptedString = Encoding.ASCII.GetBytes(GetRandomMessage());
            byte[] randomDecryptedString = Encoding.ASCII.GetBytes(GetRandomMessage());
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            var isDecrypted = false;
            Document randomDocument = new Document { FileName = randomFileName, DocumentData = randomEncryptedString };

            var expectedDependencyException =
                new DecryptionOrchestrationDependencyValidationException(
                    dependancyValidationException.InnerException as Xeption);

            this.decryptionServiceMock.Setup(service =>
              service.DecryptAsync(randomEncryptedString))
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask decryptTask = this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            DecryptionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationDependencyValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.decryptionServiceMock.Verify(service =>
              service.DecryptAsync(randomEncryptedString),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.decryptionServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }

    }
}
