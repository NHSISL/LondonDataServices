// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Services.Foundations.SubscriberPractices;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldEnsureCreatedAuditPropertiesIsSameAsStorageAsync()
        {
            // given
            SubscriberPractice inputSubscriberPractice = CreateRandomSubscriberPractice(GetRandomDateTimeOffset());
            SubscriberPractice maybeSubscriberPractice = CreateRandomSubscriberPractice(GetRandomDateTimeOffset());
            SubscriberPractice expectedSubscriberPractice = inputSubscriberPractice.DeepClone();
            expectedSubscriberPractice.CreatedDate = maybeSubscriberPractice.CreatedDate;
            expectedSubscriberPractice.CreatedBy = maybeSubscriberPractice.CreatedBy;

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            SubscriberPractice actualSubscriberPractice =
                await subscriberPracticeServiceMock.Object.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputSubscriberPractice, maybeSubscriberPractice);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice);

            subscriberPracticeServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputSubscriberPractice, maybeSubscriberPractice),
                        Times.Once());

            subscriberPracticeServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}