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
        public async Task ShouldModifyDataSetSpecificationAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DataSetSpecification randomDataSetSpecification = CreateRandomModifyDataSetSpecification(
                randomDateTimeOffset,
                dataSetSpecificationId: randomEntraUser.EntraUserId);

            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = inputDataSetSpecification.DeepClone();

            storageDataSetSpecification.UpdatedBy = randomEntraUser.EntraUserId;

            DataSetSpecification updatedDataSetSpecification = inputDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = updatedDataSetSpecification.DeepClone();

            Guid dataSetSpecificationId = inputDataSetSpecification.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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
                    Times.Exactly(2)); // CreatedDate & UpdatedDate both validated

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2)); // CreatedBy & UpdatedBy both validated

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
