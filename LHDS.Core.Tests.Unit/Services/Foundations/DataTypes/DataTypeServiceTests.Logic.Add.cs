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
        public async Task ShouldAddDataTypeAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUserId);
            DataType inputDataType = randomDataType;
            DataType storageDataType = inputDataType;
            DataType expectedDataType = storageDataType.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
				broker.ApplyAddAuditValuesAsync(inputDataType))
					.ReturnsAsync(inputDataType);

			this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDataTypeAsync(inputDataType))
                    .ReturnsAsync(storageDataType);

            // when
            DataType actualDataType = await this.dataTypeService.AddDataTypeAsync(inputDataType);

            // then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            this.securityAuditBrokerMock.Verify(broker =>
				broker.ApplyAddAuditValuesAsync(inputDataType),
					Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(inputDataType),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}