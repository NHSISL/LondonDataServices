// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldModifyDataSetSpecificationAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSetSpecification randomDataSetSpecification = CreateRandomModifyDataSetSpecification(randomDateTimeOffset);
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = inputDataSetSpecification.DeepClone();
            storageDataSetSpecification.UpdatedDate = randomDataSetSpecification.CreatedDate;
            DataSetSpecification updatedDataSetSpecification = inputDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = updatedDataSetSpecification.DeepClone();
            Guid dataSetSpecificationId = inputDataSetSpecification.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(dataSetSpecificationId))
                    .ReturnsAsync(storageDataSetSpecification);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataSetSpecificationAsync(inputDataSetSpecification))
                    .ReturnsAsync(updatedDataSetSpecification);

            // when
            DataSetSpecification actualDataSetSpecification =
                await this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(inputDataSetSpecification);

            // then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}