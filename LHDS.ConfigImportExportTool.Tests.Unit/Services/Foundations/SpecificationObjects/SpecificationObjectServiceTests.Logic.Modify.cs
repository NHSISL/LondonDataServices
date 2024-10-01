// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldModifySpecificationObjectAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject randomSpecificationObject = CreateRandomModifySpecificationObject(randomDateTimeOffset);
            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject storageSpecificationObject = inputSpecificationObject.DeepClone();
            storageSpecificationObject.UpdatedDate = randomSpecificationObject.CreatedDate;
            SpecificationObject updatedSpecificationObject = inputSpecificationObject;
            SpecificationObject expectedSpecificationObject = updatedSpecificationObject.DeepClone();
            Guid specificationObjectId = inputSpecificationObject.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(specificationObjectId))
                    .ReturnsAsync(storageSpecificationObject);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSpecificationObjectAsync(inputSpecificationObject))
                    .ReturnsAsync(updatedSpecificationObject);

            // when
            SpecificationObject actualSpecificationObject =
                await this.specificationObjectService.ModifySpecificationObjectAsync(inputSpecificationObject);

            // then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObject.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(inputSpecificationObject),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}