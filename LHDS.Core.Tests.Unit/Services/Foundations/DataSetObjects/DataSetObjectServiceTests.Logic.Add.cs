using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldAddDataSetObjectAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            SpecificationObject randomDataSetObject = CreateRandomDataSetObject(randomDateTimeOffset);
            SpecificationObject inputDataSetObject = randomDataSetObject;
            SpecificationObject storageDataSetObject = inputDataSetObject;
            SpecificationObject expectedDataSetObject = storageDataSetObject.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDataSetObjectAsync(inputDataSetObject))
                    .ReturnsAsync(storageDataSetObject);

            // when
            SpecificationObject actualDataSetObject = await this.dataSetObjectService
                .AddDataSetObjectAsync(inputDataSetObject);

            // then
            actualDataSetObject.Should().BeEquivalentTo(expectedDataSetObject);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(inputDataSetObject),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}