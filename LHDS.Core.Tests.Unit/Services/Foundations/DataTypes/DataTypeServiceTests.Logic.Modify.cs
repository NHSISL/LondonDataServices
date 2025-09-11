// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataTypes;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldModifyDataTypeAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset, randomEntraUserId);
            DataType inputDataType = randomDataType;
            DataType storageDataType = inputDataType.DeepClone();
            storageDataType.UpdatedDate = randomDataType.CreatedDate;
            DataType updatedDataType = inputDataType;
            DataType expectedDataType = updatedDataType.DeepClone();
            Guid dataTypeId = inputDataType.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(brokers =>
                brokers.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(dataTypeId))
                    .ReturnsAsync(storageDataType);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataTypeAsync(inputDataType))
                    .ReturnsAsync(updatedDataType);

            // when
            DataType actualDataType =
                await this.dataTypeService.ModifyDataTypeAsync(inputDataType);

            // then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(inputDataType.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(inputDataType),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}