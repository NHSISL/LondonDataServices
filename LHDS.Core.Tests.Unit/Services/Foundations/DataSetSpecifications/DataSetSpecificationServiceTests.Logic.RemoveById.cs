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
        public async Task ShouldRemoveDataSetSpecificationByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputDataSetSpecificationId = randomId;
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification storageDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedInputDataSetSpecification = storageDataSetSpecification;
            DataSetSpecification deletedDataSetSpecification = expectedInputDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = deletedDataSetSpecification.DeepClone();

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

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecificationId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetSpecificationAsync(expectedInputDataSetSpecification),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}