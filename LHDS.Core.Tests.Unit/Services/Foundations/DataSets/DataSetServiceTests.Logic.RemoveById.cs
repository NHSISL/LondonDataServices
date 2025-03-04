// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataSets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldRemoveDataSetByIdAsync()
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DataSet randomDataSet = CreateRandomDataSet(randomDateTimeOffset, dataSetId: randomEntraUser.EntraUserId);

            Guid inputDataSetId = randomDataSet.Id;
            DataSet storageDataSet = randomDataSet;

            DataSet dataSetWithDeleteAuditApplied = storageDataSet.DeepClone();
            dataSetWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            dataSetWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;

            DataSet updatedDataSet = dataSetWithDeleteAuditApplied;
            DataSet deletedDataSet = updatedDataSet;
            DataSet expectedDataSet = deletedDataSet.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(inputDataSetId))
                    .ReturnsAsync(storageDataSet);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataSetAsync(It.IsAny<DataSet>()))
                    .ReturnsAsync(updatedDataSet);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDataSetAsync(updatedDataSet))
                    .ReturnsAsync(deletedDataSet);

            DataSet actualDataSet = await this.dataSetService.RemoveDataSetByIdAsync(inputDataSetId);

            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(inputDataSetId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(It.IsAny<DataSet>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetAsync(updatedDataSet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}