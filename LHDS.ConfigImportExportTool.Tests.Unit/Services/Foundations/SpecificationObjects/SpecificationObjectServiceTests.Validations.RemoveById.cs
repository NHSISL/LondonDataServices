// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidSpecificationObjectId = Guid.Empty;

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.Id),
                values: "Id is required");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> removeSpecificationObjectByIdTask =
                this.specificationObjectService.RemoveSpecificationObjectByIdAsync(invalidSpecificationObjectId);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    removeSpecificationObjectByIdTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}