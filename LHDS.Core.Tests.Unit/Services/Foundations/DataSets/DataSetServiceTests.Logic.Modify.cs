// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldModifyDataSetAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomModifyDataSet(randomDateTimeOffset);
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = inputDataSet.DeepClone();
            storageDataSet.UpdatedDate = randomDataSet.CreatedDate;
            DataSet updatedDataSet = inputDataSet;
            DataSet expectedDataSet = updatedDataSet.DeepClone();
            Guid dataSetId = inputDataSet.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(dataSetId))
                    .ReturnsAsync(storageDataSet);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataSetAsync(inputDataSet))
                    .ReturnsAsync(updatedDataSet);

            // when
            DataSet actualDataSet =
                await this.dataSetService.ModifyDataSetAsync(inputDataSet);

            // then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(inputDataSet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}