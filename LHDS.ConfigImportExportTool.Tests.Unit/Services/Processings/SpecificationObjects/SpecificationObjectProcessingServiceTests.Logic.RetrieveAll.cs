// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        [Fact]
        public async Task ShouldReturnSpecificationObjects()
        {
            // given
            IQueryable<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            IQueryable<SpecificationObject> storageSpecificationObjects = randomSpecificationObjects;
            IQueryable<SpecificationObject> expectedSpecificationObjects = storageSpecificationObjects;

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjectsAsync())
                    .ReturnsAsync(storageSpecificationObjects);

            // when
            IQueryable<SpecificationObject> actualSpecificationObjects =
                await this.specificationObjectProcessingService.RetrieveAllSpecificationObjectsAsync();

            // then
            actualSpecificationObjects.Should().BeEquivalentTo(expectedSpecificationObjects);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjectsAsync(),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}