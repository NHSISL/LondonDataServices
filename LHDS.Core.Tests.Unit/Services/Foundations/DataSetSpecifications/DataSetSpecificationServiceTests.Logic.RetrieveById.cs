// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldRetrieveDataSetSpecificationByIdAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecification.Id))
                    .ReturnsAsync(storageDataSetSpecification);

            // when
            DataSetSpecification actualDataSetSpecification =
                await this.dataSetSpecificationService.RetrieveDataSetSpecificationByIdAsync(
                    inputDataSetSpecification.Id);

            // then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}