// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldEnsureCreatedAuditPropertiesIsSameAsStorageAsync()
        {
            // given
            SubscriberAgreement inputSubscriberAgreement = CreateRandomSubscriberAgreement(GetRandomDateTimeOffset());
            SubscriberAgreement maybeSubscriberAgreement = CreateRandomSubscriberAgreement(GetRandomDateTimeOffset());
            SubscriberAgreement expectedSubscriberAgreement = inputSubscriberAgreement.DeepClone();
            expectedSubscriberAgreement.CreatedDate = maybeSubscriberAgreement.CreatedDate;
            expectedSubscriberAgreement.CreatedBy = maybeSubscriberAgreement.CreatedBy;

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            SubscriberAgreement actualSubscriberAgreement =
                await subscriberAgreementServiceMock.Object.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputSubscriberAgreement, maybeSubscriberAgreement);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            subscriberAgreementServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputSubscriberAgreement, maybeSubscriberAgreement),
                        Times.Once());

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}