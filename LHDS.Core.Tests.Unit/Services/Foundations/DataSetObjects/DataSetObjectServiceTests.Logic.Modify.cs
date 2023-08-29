using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.DataSetObjects;
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
            DataSetObject randomDataSetObject = CreateRandomModifyDataSetObject(randomDateTimeOffset);
            DataSetObject inputDataSetObject = randomDataSetObject;
            DataSetObject storageDataSetObject = inputDataSetObject.DeepClone();
            storageDataSetObject.UpdatedDate = randomDataSetObject.CreatedDate;
            DataSetObject updatedDataSetObject = inputDataSetObject;
            DataSetObject expectedDataSetObject = updatedDataSetObject.DeepClone();
            Guid dataSetObjectId = inputDataSetObject.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataSetObjectAsync(inputDataSetObject))
                    .ReturnsAsync(updatedDataSetObject);

            // when
            DataSetObject actualDataSetObject =
                await this.dataSetObjectService.ModifyDataSetObjectAsync(inputDataSetObject);

            // then
            actualDataSetObject.Should().BeEquivalentTo(expectedDataSetObject);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetObjectAsync(inputDataSetObject),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}