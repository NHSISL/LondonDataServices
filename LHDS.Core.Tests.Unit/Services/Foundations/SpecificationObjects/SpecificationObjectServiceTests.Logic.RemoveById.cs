// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSpecificationObjectByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputSpecificationObjectId = randomId;
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SpecificationObject storageSpecificationObject = randomSpecificationObject;
            SpecificationObject expectedInputSpecificationObject = storageSpecificationObject;
            SpecificationObject deletedSpecificationObject = expectedInputSpecificationObject;
            SpecificationObject expectedSpecificationObject = deletedSpecificationObject.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObjectId))
                    .ReturnsAsync(storageSpecificationObject);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSpecificationObjectAsync(expectedInputSpecificationObject))
                    .ReturnsAsync(deletedSpecificationObject);

            // when
            SpecificationObject actualSpecificationObject = await this.specificationObjectService
                .RemoveSpecificationObjectByIdAsync(inputSpecificationObjectId);

            // then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObjectId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSpecificationObjectAsync(expectedInputSpecificationObject),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}