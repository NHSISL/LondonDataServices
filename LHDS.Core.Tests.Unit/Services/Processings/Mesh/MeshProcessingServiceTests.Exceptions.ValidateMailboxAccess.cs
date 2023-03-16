// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExOnValidateAccessIfDependencyValidationErrOccursAndLogItAsync(
           Xeption dependencyValidationException)
        {
            // given
            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
              service.ValidateMailboxAccessAsync())
                  .Throws(dependencyValidationException);

            // when
            ValueTask<bool> validateMailboxAccessTask =
               this.meshProcessingService.ValidateMailboxAccessAsync();

            MeshProcessingDependencyValidationException actualException =
               await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(validateMailboxAccessTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
               service.ValidateMailboxAccessAsync(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyValidationException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
