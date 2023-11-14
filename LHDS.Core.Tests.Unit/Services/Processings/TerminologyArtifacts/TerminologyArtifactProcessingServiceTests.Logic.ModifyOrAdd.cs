// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {

        [Fact]
        public async Task ShouldModifyTerminologyArtifactIfOneExistsAndNotAddAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifacts = CreateRandomTerminologyArtifact();
            TerminologyArtifact storageTerminologyArtifacts = randomTerminologyArtifacts;
            TerminologyArtifact modifiedTerminologyArtifact = storageTerminologyArtifacts.DeepClone();
            modifiedTerminologyArtifact.Name = modifiedTerminologyArtifact.Name + "Modified";
            modifiedTerminologyArtifact.IsDownloaded = false;
            TerminologyArtifact updatedTerminologyArtifacts = modifiedTerminologyArtifact.DeepClone();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(modifiedTerminologyArtifact.Id))
                    .ReturnsAsync(storageTerminologyArtifacts);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.ModifyTerminologyArtifactAsync(modifiedTerminologyArtifact))
                    .ReturnsAsync(value: updatedTerminologyArtifacts);

            // when
            await this.terminologyArtifactProcessingService.
                ModifyOrAddTerminologyArtifactAsync(modifiedTerminologyArtifact);

            // then
            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(modifiedTerminologyArtifact.Id),
                    Times.Once);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.ModifyTerminologyArtifactAsync(modifiedTerminologyArtifact),
                    Times.Once);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.AddTerminologyArtifactAsync(modifiedTerminologyArtifact),
                    Times.Never);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddTerminologyArtifactIfDataSetDoesNotExistsAsync()
        {
            // Given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            inputTerminologyArtifact.IsCore = false;
            inputTerminologyArtifact.IsDownloaded = false;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact.DeepClone();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id))
                    .ReturnsAsync(value: null);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.AddTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(value: storageTerminologyArtifact);

            // When
            await this.terminologyArtifactProcessingService.
                ModifyOrAddTerminologyArtifactAsync(inputTerminologyArtifact);

            // Then
            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id),
                    Times.Once);

            this.terminologyArtifactServiceMock.Verify(service =>
            service.AddTerminologyArtifactAsync(inputTerminologyArtifact),
            Times.Once);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.ModifyTerminologyArtifactAsync(inputTerminologyArtifact),
                    Times.Never);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
