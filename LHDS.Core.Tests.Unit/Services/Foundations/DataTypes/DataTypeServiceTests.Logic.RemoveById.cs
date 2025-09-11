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
        public async Task ShouldRemoveDataTypeByIdAsync()
        {
            //Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUserId);
            Guid inputDataTypeId = randomDataType.Id;
            DataType storageDataType = randomDataType;
            DataType dataSetWithDeleteAuditApplied = storageDataType.DeepClone();
            dataSetWithDeleteAuditApplied.UpdatedBy = randomEntraUserId.ToString();
            dataSetWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            DataType updatedDataType = dataSetWithDeleteAuditApplied;
            DataType deletedDataType = updatedDataType;
            DataType expectedDataType = deletedDataType.DeepClone();

			this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(inputDataTypeId))
                    .ReturnsAsync(storageDataType);

            this.securityAuditBrokerMock.Setup(broker =>
				broker.ApplyRemoveAuditValuesAsync(storageDataType))
					.ReturnsAsync(dataSetWithDeleteAuditApplied);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataTypeAsync(dataSetWithDeleteAuditApplied))
                    .ReturnsAsync(updatedDataType);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDataTypeAsync(updatedDataType))
                    .ReturnsAsync(deletedDataType);

            //When
            DataType actualDataType = await this.dataTypeService.RemoveDataTypeByIdAsync(inputDataTypeId);

            //Then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(inputDataTypeId),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyRemoveAuditValuesAsync(storageDataType),
					Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(dataSetWithDeleteAuditApplied),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataTypeAsync(updatedDataType),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
        }
    }
}