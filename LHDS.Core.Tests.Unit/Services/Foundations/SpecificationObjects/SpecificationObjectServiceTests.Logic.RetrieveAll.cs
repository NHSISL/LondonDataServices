// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldReturnSpecificationObjectsAsync()
        {
            // given
            IQueryable<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            IQueryable<SpecificationObject> storageSpecificationObjects = randomSpecificationObjects;
            IQueryable<SpecificationObject> expectedSpecificationObjects = storageSpecificationObjects;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSpecificationObjectsAsync())
                    .ReturnsAsync(storageSpecificationObjects);

            // when
            IQueryable<SpecificationObject> actualSpecificationObjects =
                await this.specificationObjectService.RetrieveAllSpecificationObjectsAsync();

            // then
            actualSpecificationObjects.Should().BeEquivalentTo(expectedSpecificationObjects);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSpecificationObjectsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}