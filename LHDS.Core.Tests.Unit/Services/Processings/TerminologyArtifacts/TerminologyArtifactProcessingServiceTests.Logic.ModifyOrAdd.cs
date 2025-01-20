// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ShouldModifyTerminologyArtifactIfOneExistsAndNotAddAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifacts = CreateRandomTerminologyArtifact();
            TerminologyArtifact storageTerminologyArtifacts = randomTerminologyArtifacts;
            TerminologyArtifact modifiedTerminologyArtifact = storageTerminologyArtifacts.DeepClone();
            modifiedTerminologyArtifact.Name = modifiedTerminologyArtifact.Name + "Modified";
            modifiedTerminologyArtifact.UpdatedDate = randomDateTimeOffset;
            modifiedTerminologyArtifact.IsDownloaded = false;
            TerminologyArtifact updatedTerminologyArtifacts = modifiedTerminologyArtifact.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            List<TerminologyArtifact> terminologyArtifacts =
                new List<TerminologyArtifact> { storageTerminologyArtifacts };

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ReturnsAsync(value: terminologyArtifacts.AsQueryable());

            this.terminologyArtifactServiceMock.Setup(service =>
                service.ModifyTerminologyArtifactAsync(modifiedTerminologyArtifact))
                    .ReturnsAsync(value: updatedTerminologyArtifacts);

            // when
            await this.terminologyArtifactProcessingService.
                ModifyOrAddTerminologyArtifactAsync(modifiedTerminologyArtifact);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifactsAsync(),
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
        public async System.Threading.Tasks.Task ShouldAddTerminologyArtifactIfTerminologyArtifactDoesNotExistsAsync()
        {
            // Given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            inputTerminologyArtifact.IsDownloaded = false;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact.DeepClone();
            List<TerminologyArtifact> terminologyArtifacts = new List<TerminologyArtifact>();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ReturnsAsync(value: terminologyArtifacts.AsQueryable());

            this.terminologyArtifactServiceMock.Setup(service =>
                service.AddTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(value: storageTerminologyArtifact);

            // When
            await this.terminologyArtifactProcessingService.
                ModifyOrAddTerminologyArtifactAsync(inputTerminologyArtifact);

            // Then
            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifactsAsync(),
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
