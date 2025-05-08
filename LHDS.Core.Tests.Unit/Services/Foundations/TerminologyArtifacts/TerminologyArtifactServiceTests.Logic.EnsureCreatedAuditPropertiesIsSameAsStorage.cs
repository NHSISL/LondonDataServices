// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldEnsureCreatedAuditPropertiesIsSameAsStorageAsync()
        {
            // given
            TerminologyArtifact inputTerminologyArtifact = CreateRandomTerminologyArtifact(GetRandomDateTimeOffset());
            TerminologyArtifact maybeTerminologyArtifact = CreateRandomTerminologyArtifact(GetRandomDateTimeOffset());
            TerminologyArtifact expectedTerminologyArtifact = inputTerminologyArtifact.DeepClone();
            expectedTerminologyArtifact.CreatedDate = maybeTerminologyArtifact.CreatedDate;
            expectedTerminologyArtifact.CreatedBy = maybeTerminologyArtifact.CreatedBy;

            var terminologyArtifactServiceMock = new Mock<TerminologyArtifactService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            TerminologyArtifact actualTerminologyArtifact =
                await terminologyArtifactServiceMock.Object.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputTerminologyArtifact, maybeTerminologyArtifact);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            terminologyArtifactServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputTerminologyArtifact, maybeTerminologyArtifact),
                        Times.Once());

            terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}