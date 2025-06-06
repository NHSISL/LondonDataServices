// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentDataSetSpecificationProcessingException =
                new InvalidArgumentDataSetSpecificationProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentDataSetSpecificationProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedDataSetSpecificationProcessingValidationException =
                new DataSetSpecificationProcessingValidationException(
                    message: "DataSetSpecification processing validation error occurred, please try again.",
                    innerException: invalidArgumentDataSetSpecificationProcessingException);

            // when
            ValueTask<DataSetSpecification> RetrieveDataSetSpecificationTask =
                this.dataSetSpecificationProcessingService.RetrieveDataSetSpecificationByIdAsync(invalidId);

            DataSetSpecificationProcessingValidationException actualDataSetSpecificationProcessingValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingValidationException>(
                    RetrieveDataSetSpecificationTask.AsTask);

            //then
            actualDataSetSpecificationProcessingValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationProcessingValidationException))),
                        Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
