// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisation;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Documents.Exceptions;
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

            var expectedDocumentProcessingDependencyValidationException =
                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressNormalisationServiceMock.Setup(service =>
                service.GetNormalisedAddress(inputAddress))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<AddressNormalisation> getNormalisedAddressTask =
                this.addressNormalisationProcessingService.GetNormalisedAddress(inputAddress);

            DocumentProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DocumentProcessingDependencyValidationException>(
                    getNormalisedAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingDependencyValidationException);

            this.addressNormalisationServiceMock.Verify(service =>
                service.GetNormalisedAddress(inputAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDocumentProcessingDependencyValidationException))),
                         Times.Once);

            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
