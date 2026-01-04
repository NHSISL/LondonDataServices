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
            string randomUserId = GetRandomStringWithLengthOf(50);

            SpecificationObject randomSpecificationObject = 
                CreateRandomSpecificationObject(randomDateTimeOffset, randomUserId);

            Guid inputSpecificationObjectId = randomSpecificationObject.Id;
            SpecificationObject storageSpecificationObject = randomSpecificationObject;
            SpecificationObject dataSetWithDeleteAuditApplied = storageSpecificationObject.DeepClone();
            dataSetWithDeleteAuditApplied.UpdatedBy = randomUserId.ToString();
            dataSetWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            SpecificationObject updatedSpecificationObject = dataSetWithDeleteAuditApplied;
            SpecificationObject deletedSpecificationObject = updatedSpecificationObject;
            SpecificationObject expectedSpecificationObject = deletedSpecificationObject.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObjectId))
                    .ReturnsAsync(storageSpecificationObject);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyRemoveAuditValuesAsync(storageSpecificationObject))
                    .ReturnsAsync(dataSetWithDeleteAuditApplied);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSpecificationObjectAsync(dataSetWithDeleteAuditApplied))
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyRemoveAuditValuesAsync(storageSpecificationObject),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(dataSetWithDeleteAuditApplied),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSpecificationObjectAsync(updatedSpecificationObject),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
        }
    }
}