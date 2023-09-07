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
        public async Task ShouldModifyDataSetObjectAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject randomDataSetObject = CreateRandomModifyDataSetObject(randomDateTimeOffset);
            SpecificationObject inputDataSetObject = randomDataSetObject;
            SpecificationObject storageDataSetObject = inputDataSetObject.DeepClone();
            storageDataSetObject.UpdatedDate = randomDataSetObject.CreatedDate;
            SpecificationObject updatedDataSetObject = inputDataSetObject;
            SpecificationObject expectedDataSetObject = updatedDataSetObject.DeepClone();
            Guid dataSetObjectId = inputDataSetObject.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(dataSetObjectId))
                    .ReturnsAsync(storageDataSetObject);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataSetObjectAsync(inputDataSetObject))
                    .ReturnsAsync(updatedDataSetObject);

            // when
            SpecificationObject actualDataSetObject =
                await this.dataSetObjectService.ModifyDataSetObjectAsync(inputDataSetObject);

            // then
            actualDataSetObject.Should().BeEquivalentTo(expectedDataSetObject);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(inputDataSetObject.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetObjectAsync(inputDataSetObject),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}