// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact modifiedTerminologyArtifact = storageTerminologyArtifact.DeepClone();
            modifiedTerminologyArtifact.Name = modifiedTerminologyArtifact.Name + "Modified";
            modifiedTerminologyArtifact.UpdatedDate = randomDateTimeOffset;
            TerminologyArtifact updatedTerminologyArtifacts = modifiedTerminologyArtifact.DeepClone();
            List<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomTerminologyArtifacts().ToList();
            randomTerminologyArtifacts[0] = randomTerminologyArtifact;
            IQueryable<TerminologyArtifact> outputTerminologyArtifacts = randomTerminologyArtifacts.AsQueryable();
            IQueryable<TerminologyArtifact> expectedTerminologyArtifacts = outputTerminologyArtifacts.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Returns(outputTerminologyArtifacts);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.ModifyTerminologyArtifactAsync(modifiedTerminologyArtifact))
                    .ReturnsAsync(value: updatedTerminologyArtifacts);

            // when
            await this.terminologyArtifactProcessingService.
                ModifyOrAddTerminologyArtifactAsync(modifiedTerminologyArtifact);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
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
        public async Task ShouldAddTerminologyArtifactIfTerminologyArtifactDoesNotExistsAsync()
        {
            // Given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            inputTerminologyArtifact.IsDownloaded = false;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact.DeepClone();
            IQueryable<TerminologyArtifact> retrievedTerminologyArtifacts = CreateRandomTerminologyArtifacts();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Returns(value: retrievedTerminologyArtifacts);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.AddTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(value: storageTerminologyArtifact);

            // When
            await this.terminologyArtifactProcessingService.
                ModifyOrAddTerminologyArtifactAsync(inputTerminologyArtifact);

            // Then
            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
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
