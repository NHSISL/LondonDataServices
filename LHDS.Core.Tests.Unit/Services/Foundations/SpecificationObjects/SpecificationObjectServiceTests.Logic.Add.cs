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
        public async Task ShouldAddSpecificationObjectAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject(randomDateTimeOffset);
            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject storageSpecificationObject = inputSpecificationObject;
            SpecificationObject expectedSpecificationObject = storageSpecificationObject.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSpecificationObjectAsync(inputSpecificationObject))
                    .ReturnsAsync(storageSpecificationObject);

            // when
            SpecificationObject actualSpecificationObject = await this.specificationObjectService
                .AddSpecificationObjectAsync(inputSpecificationObject);

            // then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(inputSpecificationObject),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}