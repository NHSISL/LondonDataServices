using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidDataSetObjectId = Guid.Empty;

            var invalidDataSetObjectException = 
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.Id),
                values: "Id is required");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            // when
            ValueTask<SpecificationObject> retrieveDataSetObjectByIdTask =
                this.dataSetObjectService.RetrieveDataSetObjectByIdAsync(invalidDataSetObjectId);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    retrieveDataSetObjectByIdTask.AsTask);

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfDataSetObjectIsNotFoundAndLogItAsync()
        {
            //given
            Guid someDataSetObjectId = Guid.NewGuid();
            SpecificationObject noDataSetObject = null;

            var notFoundDataSetObjectException =
                new NotFoundDataSetObjectException(someDataSetObjectId);

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: notFoundDataSetObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noDataSetObject);

            //when
            ValueTask<SpecificationObject> retrieveDataSetObjectByIdTask =
                this.dataSetObjectService.RetrieveDataSetObjectByIdAsync(someDataSetObjectId);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    retrieveDataSetObjectByIdTask.AsTask);

            //then
            actualDataSetObjectValidationException.Should().BeEquivalentTo(expectedDataSetObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}