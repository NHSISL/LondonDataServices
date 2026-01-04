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
        public async Task ShouldModifySpecificationObjectAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SpecificationObject randomSpecificationObject = 
                CreateRandomModifySpecificationObject(randomDateTimeOffset, randomUserId);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject storageSpecificationObject = inputSpecificationObject.DeepClone();
            storageSpecificationObject.UpdatedDate = randomSpecificationObject.CreatedDate;
            SpecificationObject updatedSpecificationObject = inputSpecificationObject;
            SpecificationObject expectedSpecificationObject = updatedSpecificationObject.DeepClone();
            Guid specificationObjectId = inputSpecificationObject.Id;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputSpecificationObject))
                    .ReturnsAsync(inputSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputSpecificationObject),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(inputSpecificationObject.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(inputSpecificationObject),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}