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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidSpecificationObjectId = Guid.Empty;

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
            ValueTask<SpecificationObject> retrieveSpecificationObjectByIdTask =
                this.specificationObjectService.RetrieveSpecificationObjectByIdAsync(invalidSpecificationObjectId);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    retrieveSpecificationObjectByIdTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfSpecificationObjectIsNotFoundAndLogItAsync()
        {
            //given
            Guid someSpecificationObjectId = Guid.NewGuid();
            SpecificationObject noSpecificationObject = null;

            var notFoundSpecificationObjectException =
                new NotFoundSpecificationObjectException(someSpecificationObjectId);

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: notFoundSpecificationObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noSpecificationObject);

            //when
            ValueTask<SpecificationObject> retrieveSpecificationObjectByIdTask =
                this.specificationObjectService.RetrieveSpecificationObjectByIdAsync(someSpecificationObjectId);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    retrieveSpecificationObjectByIdTask.AsTask);

            //then
            actualSpecificationObjectValidationException.Should().BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}