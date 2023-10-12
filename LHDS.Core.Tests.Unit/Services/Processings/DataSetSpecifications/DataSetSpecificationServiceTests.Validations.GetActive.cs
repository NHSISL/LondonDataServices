// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnGetActiveIfSupplierIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidSupplierId = Guid.Empty;

            var invalidArgumentDataSetSpecificationProcessingException =
                new InvalidArgumentDataSetSpecificationProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentDataSetSpecificationProcessingException.AddData(
                key: "SupplierId",
                values: "Id is required");

            var expectedDataSetSpecificationProcessingValidationException =
                new DataSetSpecificationProcessingValidationException(
                    message: "DataSetSpecification processing validation error occurred, please try again.",
                    innerException: invalidArgumentDataSetSpecificationProcessingException);

            // when
            ValueTask<DataSetSpecification> RetrieveDataSetSpecificationTask =
                this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(invalidSupplierId);

            DataSetSpecificationProcessingValidationException actualDataSetSpecificationProcessingValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingValidationException>(
                    RetrieveDataSetSpecificationTask.AsTask);

            //then
            actualDataSetSpecificationProcessingValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationProcessingValidationException))),
                        Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnGetActiveIfCountIsZeroAndLogItAsync()
        {
            // given
            Guid randomSupplierId = Guid.NewGuid();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplierId);

            IQueryable<DataSetSpecification> randomDataSetSpecifications =
                CreateRandomDataSetSpecifications(dataSet: randomDataSet, dataSetId: randomDataSet.Id, count: 0);

            IQueryable<DataSetSpecification> storageDataSetSpecifications = randomDataSetSpecifications;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = storageDataSetSpecifications.DeepClone();

            this.dataSetSpecificationServiceMock.Setup(broker =>
                broker.RetrieveAllDataSetSpecifications())
                    .Returns(storageDataSetSpecifications);

            var invalidCountDataSetSpecificationProcessingException =
                new InvalidCountDataSetSpecificationProcessingException(
                    message: "Expected DataSetSpecification count to be one.");


            var expectedDataSetSpecificationProcessingValidationException =
                new DataSetSpecificationProcessingValidationException(
                    message: "DataSetSpecification processing validation error occurred, please try again.",
                    innerException: invalidCountDataSetSpecificationProcessingException);

            // when
            ValueTask<DataSetSpecification> RetrieveDataSetSpecificationTask =
                this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(randomSupplierId);

            DataSetSpecificationProcessingValidationException actualDataSetSpecificationProcessingValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingValidationException>(
                    RetrieveDataSetSpecificationTask.AsTask);

            //then
            actualDataSetSpecificationProcessingValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationProcessingValidationException);

            this.dataSetSpecificationServiceMock.Verify(broker =>
                broker.RetrieveAllDataSetSpecifications(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationProcessingValidationException))),
                        Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
