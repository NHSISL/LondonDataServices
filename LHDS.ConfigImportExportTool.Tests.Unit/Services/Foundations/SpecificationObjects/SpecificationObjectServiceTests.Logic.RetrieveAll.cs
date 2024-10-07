// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
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
                broker.SelectAllSpecificationObjectsAsync())
                    .ReturnsAsync(storageSpecificationObjects);

            // when
            ValueTask<IQueryable<SpecificationObject>> actualSpecificationObjects =
                this.specificationObjectService.RetrieveAllSpecificationObjectsAsync();

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