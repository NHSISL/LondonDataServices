// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public void ShouldReturnDataTypes()
        {
            // given
            IQueryable<DataType> randomDataTypes = CreateRandomDataTypes();
            IQueryable<DataType> storageDataTypes = randomDataTypes;
            IQueryable<DataType> expectedDataTypes = storageDataTypes;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataTypes())
                    .Returns(storageDataTypes);

            // when
            IQueryable<DataType> actualDataTypes =
                this.dataTypeService.RetrieveAllDataTypes();

            // then
            actualDataTypes.Should().BeEquivalentTo(expectedDataTypes);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataTypes(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}