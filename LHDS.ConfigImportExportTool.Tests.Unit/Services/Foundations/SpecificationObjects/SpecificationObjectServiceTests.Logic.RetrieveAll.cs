// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public void ShouldReturnSpecificationObjects()
        {
            // given
            IQueryable<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            IQueryable<SpecificationObject> storageSpecificationObjects = randomSpecificationObjects;
            IQueryable<SpecificationObject> expectedSpecificationObjects = storageSpecificationObjects;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSpecificationObjects())
                    .ReturnsAsync(storageSpecificationObjects);

            // when
            IQueryable<SpecificationObject> actualSpecificationObjects =
                this.specificationObjectService.RetrieveAllSpecificationObjects();

            // then
            actualSpecificationObjects.Should().BeEquivalentTo(expectedSpecificationObjects);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSpecificationObjects(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}