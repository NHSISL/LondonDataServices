// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsNullAndLogItAsync()
        {
            // given
            DataSetSpecification nullDataSetSpecification = null;

            var nullDataSetSpecificationException =
                new NullDataSetSpecificationException(message: "DataSetSpecification is null.");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: nullDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(nullDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            var invalidDataSetSpecification = new DataSetSpecification
            {
                SupplierSpecificationVersion = invalidText,
                OurSpecificationVersion = invalidText,
                DataSetId = Guid.Empty,
                Id = Guid.Empty
            };

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.Id),
                values: "Id is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.DataSetId),
                values: "Id is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.SupplierSpecificationVersion),
                values: "Text is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.OurSpecificationVersion),
                values: "Text is required");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsInvalidLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DataSetSpecification invalidDataSetSpecification =
                CreateRandomDataSetSpecification(randomDateTimeOffset);

            invalidDataSetSpecification.SupplierSpecificationVersion = GetRandomString(11);
            invalidDataSetSpecification.OurSpecificationVersion = GetRandomString(11);

            // CreatedBy and UpdatedBy are now populated by audit, so you don't set them manually anymore.
            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.SupplierSpecificationVersion),
                values: "Text is exceeding max length");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.OurSpecificationVersion),
                values: "Text is exceeding max length");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            DateTimeOffset invalidDate = randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);
            DateTimeOffset startDate = randomDateTimeOffset.AddSeconds(-90);
            DateTimeOffset endDate = randomDateTimeOffset.AddSeconds(0);

            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(invalidDate);
            randomDataSetSpecification.CreatedBy = randomEntraUser.EntraUserId;
            randomDataSetSpecification.UpdatedBy = randomEntraUser.EntraUserId;

            DataSetSpecification invalidDataSetSpecification = randomDataSetSpecification;
            invalidDataSetSpecification.CreatedDate = invalidDate;
            invalidDataSetSpecification.UpdatedDate = invalidDate;

            var dataSetSpecificationServiceMock = new Mock<DataSetSpecificationService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.securityBrokerMock.Object)
            {
                CallBase = true
            };

            dataSetSpecificationServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidDataSetSpecification))
                    .ReturnsAsync(invalidDataSetSpecification);

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedDate),
                values: $"Date is not recent. Expected a value between {startDate} and {endDate} but found {invalidDate}");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                dataSetSpecificationServiceMock.Object.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}