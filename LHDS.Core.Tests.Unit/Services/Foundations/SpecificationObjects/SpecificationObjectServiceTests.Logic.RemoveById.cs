// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            SpecificationObject randomSpecificationObject = 
                CreateRandomSpecificationObject(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Guid inputSpecificationObjectId = randomSpecificationObject.Id;
            SpecificationObject storageSpecificationObject = randomSpecificationObject;
            SpecificationObject dataSetWithDeleteAuditApplied = storageSpecificationObject.DeepClone();
            dataSetWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            dataSetWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            SpecificationObject updatedSpecificationObject = dataSetWithDeleteAuditApplied;
            SpecificationObject deletedSpecificationObject = updatedSpecificationObject;
            SpecificationObject expectedSpecificationObject = deletedSpecificationObject.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObjectId))
                    .ReturnsAsync(storageSpecificationObject);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSpecificationObjectAsync(randomSpecificationObject))
                    .ReturnsAsync(updatedSpecificationObject);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSpecificationObjectAsync(updatedSpecificationObject))
                    .ReturnsAsync(deletedSpecificationObject);

            // when
            SpecificationObject actualSpecificationObject = await this.specificationObjectService
                .RemoveSpecificationObjectByIdAsync(inputSpecificationObjectId);

            // then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObjectId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(randomSpecificationObject),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSpecificationObjectAsync(updatedSpecificationObject),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}