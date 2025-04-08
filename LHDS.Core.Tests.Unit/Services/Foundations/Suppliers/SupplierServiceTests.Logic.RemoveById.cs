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
        public async Task ShouldRemoveSupplierByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Guid inputSupplierId = randomSupplier.Id;
            Supplier storageSupplier = randomSupplier;
            Supplier ingestionTrackingWithDeleteAuditApplied = storageSupplier.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            Supplier updatedSupplier = storageSupplier;
            Supplier deletedSupplier = updatedSupplier;
            Supplier expectedSupplier = deletedSupplier.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(inputSupplierId))
                    .ReturnsAsync(storageSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSupplierAsync(randomSupplier))
                    .ReturnsAsync(updatedSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSupplierAsync(updatedSupplier))
                    .ReturnsAsync(deletedSupplier);

            // when
            Supplier actualSupplier = await this.supplierService
                .RemoveSupplierByIdAsync(inputSupplierId);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(inputSupplierId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(randomSupplier),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSupplierAsync(updatedSupplier),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}