// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AssignAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Assigns
{
    public partial class AssignServiceTests
    {
        [Fact]
        public async Task ShouldMatchAddressAsync()
        {
            // Given
            string randomAddress = GetRandomString();
            string inputAddress = randomAddress;
            AssignAddress randomAssignAddress = CreateRandomAssignAddress();
            AssignAddress storageAssignAddress = randomAssignAddress;
            AssignAddress expectedAssignAddress = storageAssignAddress;

            this.assignBrokerMock.Setup(service =>
                service.MatchAddressAsync(inputAddress))
                    .ReturnsAsync(storageAssignAddress);

            // When
            AssignAddress actualAssignAddress = await this.assignService
                .MatchAddressAsync(inputAddress);

            // Then
            actualAssignAddress.Should().BeEquivalentTo(expectedAssignAddress);

            this.assignBrokerMock.Verify(service =>
                service.MatchAddressAsync(inputAddress),
                    Times.Once);

            this.assignBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
