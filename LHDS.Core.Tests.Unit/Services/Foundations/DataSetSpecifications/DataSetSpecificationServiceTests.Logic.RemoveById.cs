// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldRemoveDataSetSpecificationByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            Guid randomId = Guid.NewGuid();
            Guid inputDataSetSpecificationId = randomId;

            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(
                randomDateTimeOffset, randomEntraUserId);

            DataSetSpecification storageDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedInputDataSetSpecification = storageDataSetSpecification;
            DataSetSpecification deletedDataSetSpecification = expectedInputDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = deletedDataSetSpecification.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyRemoveAuditValuesAsync(randomDataSetSpecification))
                    .ReturnsAsync(randomDataSetSpecification);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecificationId))
                    .ReturnsAsync(storageDataSetSpecification);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDataSetSpecificationAsync(expectedInputDataSetSpecification))
                    .ReturnsAsync(deletedDataSetSpecification);

            // when
            DataSetSpecification actualDataSetSpecification = await this.dataSetSpecificationService
                .RemoveDataSetSpecificationByIdAsync(inputDataSetSpecificationId);

            // then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyRemoveAuditValuesAsync(expectedInputDataSetSpecification),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecificationId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetSpecificationAsync(expectedInputDataSetSpecification),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}