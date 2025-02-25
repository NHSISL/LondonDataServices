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
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset, randomEntraUser.EntraUserId);
            DataType inputDataType = randomDataType;
            DataType storageDataType = inputDataType.DeepClone();
            storageDataType.UpdatedDate = randomDataType.CreatedDate;
            DataType updatedDataType = inputDataType;
            DataType expectedDataType = updatedDataType.DeepClone();
            Guid dataTypeId = inputDataType.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(brokers =>
                brokers.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(inputDataType.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(inputDataType),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}