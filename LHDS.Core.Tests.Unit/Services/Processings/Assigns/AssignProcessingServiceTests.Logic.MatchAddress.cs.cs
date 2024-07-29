// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AssignAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Assigns
{
    public partial class AssignProcessingServiceTests
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

            this.assignServiceMock.Setup(service =>
                service.MatchAddressAsync(inputAddress))
                    .ReturnsAsync(storageAssignAddress);

            // When
            AssignAddress actualAssignAddress = await this.assignProcessingService
                .MatchAddressAsync(inputAddress);

            // Then
            actualAssignAddress.Should().BeEquivalentTo(expectedAssignAddress);

            this.assignServiceMock.Verify(service =>
                service.MatchAddressAsync(inputAddress),
                    Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
