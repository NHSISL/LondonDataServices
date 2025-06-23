// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldEnsureCreatedAuditPropertiesIsSameAsStorageAsync()
        {
            // given
            TerminologyPoll inputTerminologyPoll = CreateRandomTerminologyPoll(GetRandomDateTimeOffset());
            TerminologyPoll maybeTerminologyPoll = CreateRandomTerminologyPoll(GetRandomDateTimeOffset());
            TerminologyPoll expectedTerminologyPoll = inputTerminologyPoll.DeepClone();
            expectedTerminologyPoll.CreatedDate = maybeTerminologyPoll.CreatedDate;
            expectedTerminologyPoll.CreatedBy = maybeTerminologyPoll.CreatedBy;

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            TerminologyPoll actualTerminologyPoll =
                await terminologyPollServiceMock.Object.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputTerminologyPoll, maybeTerminologyPoll);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            terminologyPollServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputTerminologyPoll, maybeTerminologyPoll),
                        Times.Once());

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}