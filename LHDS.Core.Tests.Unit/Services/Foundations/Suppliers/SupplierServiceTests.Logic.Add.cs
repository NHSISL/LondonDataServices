// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Suppliers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldAddSupplierAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            Supplier randomSupplier = CreateRandomSupplier(
                randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier inputSupplier = randomSupplier;
            Supplier storageSupplier = inputSupplier;
            Supplier expectedSupplier = storageSupplier.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(inputSupplier.Id))
                    .Returns(null);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSupplierAsync(inputSupplier))
                    .ReturnsAsync(storageSupplier);

            // when
            Supplier actualSupplier = await this.supplierService.AddSupplierAsync(inputSupplier);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(inputSupplier.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(inputSupplier),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}