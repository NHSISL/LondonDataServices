// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfResolvedAddressIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress nullResolvedAddress = null;

            var nullResolvedAddressException =
                new NullResolvedAddressException(message: "Resolved address is null.");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: nullResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.AddResolvedAddressAsync(nullResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfResolvedAddresssIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddress invalidResolvedAddress = new ResolvedAddress
            {
                UnstructuredPostalAddress = invalidText,
            };

            var resolvedAddressServiceMock = new Mock<ResolvedAddressService>(
                storageBrokerMock.Object,
                identifierBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object, 
                auditBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.Id),
                values: "Id is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UniqueReference),
                values: "Id is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UnstructuredPostalAddress),
                values: "Text is required");

            invalidResolvedAddressException.AddData(
                 key: nameof(ResolvedAddress.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidResolvedAddress.CreatedBy}'."
                ]);

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values: "Date is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedBy),
                values: "Text is required");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                resolvedAddressServiceMock.Object.AddResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            resolvedAddressServiceMock.Verify(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddress randomResolvedAddress =
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;
            invalidResolvedAddress.CreatedDate = GetRandomDateTimeOffset();
            invalidResolvedAddress.UpdatedDate = GetRandomDateTimeOffset();

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values: $"Date is not the same as {nameof(ResolvedAddress.CreatedDate)}");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedDate),
                values: $"Date is not recent");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            var resolvedAddressServiceMock = new Mock<ResolvedAddressService>(
                storageBrokerMock.Object,
                identifierBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object, 
                auditBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                resolvedAddressServiceMock.Object.AddResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            resolvedAddressServiceMock.Verify(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress), 
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddress randomResolvedAddress =
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;
            invalidResolvedAddress.CreatedBy = GetRandomString();
            invalidResolvedAddress.UpdatedBy = GetRandomString();

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedBy),
                values: $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidResolvedAddress.CreatedBy}'.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedBy),
                values: $"Text is not the same as {nameof(ResolvedAddress.CreatedBy)}");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            var resolvedAddressServiceMock = new Mock<ResolvedAddressService>(
                storageBrokerMock.Object,
                identifierBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object,
                auditBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                resolvedAddressServiceMock.Object.AddResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            resolvedAddressServiceMock.Verify(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfHashGreaterThanMaxLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddress invalidResolvedAddress =
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidResolvedAddress.HashedUnstructuredPostalAddress =
                GetRandomHexString(33).ToCharArray();

            var resolvedAddressServiceMock = new Mock<ResolvedAddressService>(
                storageBrokerMock.Object,
                identifierBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object,
                auditBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.HashedUnstructuredPostalAddress),
                values: "Char array length should not be greater than 32");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                resolvedAddressServiceMock.Object.AddResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            resolvedAddressServiceMock.Verify(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset invalidDateTime = randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            ResolvedAddress randomResolvedAddress =
                CreateRandomResolvedAddress(invalidDateTime, randomEntraUser.EntraUserId);

            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedDate),
                values: "Date is not recent");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            var resolvedAddressServiceMock = new Mock<ResolvedAddressService>(
                storageBrokerMock.Object,
                identifierBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object,
                auditBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                resolvedAddressServiceMock.Object.AddResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            resolvedAddressServiceMock.Verify(service =>
                service.ApplyAddAuditAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}