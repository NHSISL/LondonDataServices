using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSubscriberAgreementByIdAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id))
                    .ReturnsAsync(storageSubscriberAgreement);

            // when
            SubscriberAgreement actualSubscriberAgreement =
                await this.subscriberAgreementService.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}