// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSpecificationObjectByIdAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject storageSpecificationObject = randomSpecificationObject;
            SpecificationObject expectedSpecificationObject = storageSpecificationObject.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObject.Id))
                    .ReturnsAsync(storageSpecificationObject);

            // when
            SpecificationObject actualSpecificationObject =
                await this.specificationObjectService.RetrieveSpecificationObjectByIdAsync(inputSpecificationObject.Id);

            // then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObject.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}