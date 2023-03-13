// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var randomString = GetRandomString();
            var randomBytes = Encoding.ASCII.GetBytes(GetRandomString());
            var randomMessage = GetRandomString();

            Document inputDocument = new Document
            {
                FileName = randomString,
                DocumentData = randomBytes
            };

            var expectedDocumentProcessingDependencyValidationException = 
                new DocumentProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.documentServiceMock.Setup(service => 
                service.RetrieveDocumentByFileNameAsync(inputDocument.FileName))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<Document> retrieveDocumentTask = 
                this.documentProcessingService.RetrieveDocumentByFileNameAsync(inputDocument.FileName);

            DocumentProcessingDependencyValidationException actualException = 
                await Assert.ThrowsAsync<DocumentProcessingDependencyValidationException>(retrieveDocumentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDocumentProcessingDependencyValidationException);

            this.documentServiceMock.Verify(service => 
                service.RetrieveDocumentByFileNameAsync(inputDocument.FileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDocumentProcessingDependencyValidationException))),
                         Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
