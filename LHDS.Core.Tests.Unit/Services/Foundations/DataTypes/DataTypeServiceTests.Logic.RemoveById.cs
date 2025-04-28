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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Guid inputDataTypeId = randomDataType.Id;
            DataType storageDataType = randomDataType;
            DataType dataSetWithDeleteAuditApplied = storageDataType.DeepClone();
            dataSetWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            dataSetWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            DataType updatedDataType = dataSetWithDeleteAuditApplied;
            DataType deletedDataType = updatedDataType;
            DataType expectedDataType = deletedDataType.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(inputDataTypeId))
                    .ReturnsAsync(storageDataType);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataTypeAsync(randomDataType))
                    .ReturnsAsync(updatedDataType);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDataTypeAsync(updatedDataType))
                    .ReturnsAsync(deletedDataType);

            DataType actualDataType = await this.dataTypeService.RemoveDataTypeByIdAsync(inputDataTypeId);

            actualDataType.Should().BeEquivalentTo(expectedDataType);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(inputDataTypeId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(randomDataType),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataTypeAsync(updatedDataType),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}