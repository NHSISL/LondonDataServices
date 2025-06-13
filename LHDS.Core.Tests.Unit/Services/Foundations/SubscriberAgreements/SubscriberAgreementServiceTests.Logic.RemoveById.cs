// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSubscriberAgreementByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputSubscriberAgreementId = randomId;
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedInputSubscriberAgreement = storageSubscriberAgreement;
            SubscriberAgreement deletedSubscriberAgreement = expectedInputSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = deletedSubscriberAgreement.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreementId))
                    .ReturnsAsync(storageSubscriberAgreement);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSubscriberAgreementAsync(expectedInputSubscriberAgreement))
                    .ReturnsAsync(deletedSubscriberAgreement);

            // when
            SubscriberAgreement actualSubscriberAgreement = await this.subscriberAgreementService
                .RemoveSubscriberAgreementByIdAsync(inputSubscriberAgreementId);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreementId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSubscriberAgreementAsync(expectedInputSubscriberAgreement),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}