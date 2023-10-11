// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByFileNameIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someFileName = GetRandomString();

            var expectedDownloadProcessingDependencyValidationException =
                new DownloadProcessingDependencyValidationException(
                    message: "Download processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<Document> downloadRetrieveByIdTask =
                this.downloadProcessingService.RetrieveDownloadByFileNameAsync(someFileName);

            DownloadProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DownloadProcessingDependencyValidationException>(
                    downloadRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadProcessingDependencyValidationException);

            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDownloadProcessingDependencyValidationException))),
                         Times.Once);

            this.downloadServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}