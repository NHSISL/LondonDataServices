// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldReturnDataTypesAsync()
        {
            // given
            IQueryable<DataType> randomDataTypes = CreateRandomDataTypes();
            IQueryable<DataType> storageDataTypes = randomDataTypes;
            IQueryable<DataType> expectedDataTypes = storageDataTypes;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataTypesAsync())
                    .ReturnsAsync(storageDataTypes);

            // when
            IQueryable<DataType> actualDataTypes =
                await this.dataTypeService.RetrieveAllDataTypesAsync();

            // then
            actualDataTypes.Should().BeEquivalentTo(expectedDataTypes);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataTypesAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}