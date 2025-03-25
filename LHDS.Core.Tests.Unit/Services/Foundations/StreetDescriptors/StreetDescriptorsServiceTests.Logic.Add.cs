// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.StreetDescriptors;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.StreetDescriptors
{
    public partial class StreetDescriptorServiceTests
    {
        [Fact]
        public async Task ShouldAddStreetDescriptorAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            
            StreetDescriptor randomStreetDescriptor = 
                    CreateRandomStreetDescriptor(randomDateTimeOffset, randomEntraUser.EntraUserId);

            StreetDescriptor inputStreetDescriptor = randomStreetDescriptor;
            StreetDescriptor storageStreetDescriptor = inputStreetDescriptor;
            StreetDescriptor expectedStreetDescriptor = storageStreetDescriptor.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStreetDescriptorAsync(inputStreetDescriptor))
                    .ReturnsAsync(storageStreetDescriptor);

            // when
            StreetDescriptor actualStreetDescriptor = await this.streetDescriptorService
                .AddStreetDescriptorAsync(inputStreetDescriptor);

            // then
            actualStreetDescriptor.Should().BeEquivalentTo(expectedStreetDescriptor);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(1));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(1));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStreetDescriptorAsync(inputStreetDescriptor),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}