using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldAddDataSetSpecificationAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDateTimeOffset);
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = inputDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDataSetSpecificationAsync(inputDataSetSpecification))
                    .ReturnsAsync(storageDataSetSpecification);

            // when
            DataSetSpecification actualDataSetSpecification = await this.dataSetSpecificationService
                .AddDataSetSpecificationAsync(inputDataSetSpecification);

            // then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}