// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Suppliers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldModifySupplierAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Supplier randomSupplier = CreateRandomModifySupplier(randomDateTimeOffset);
            Supplier inputSupplier = randomSupplier;
            Supplier storageSupplier = inputSupplier.DeepClone();
            storageSupplier.UpdatedDate = randomSupplier.CreatedDate;
            Supplier updatedSupplier = inputSupplier;
            Supplier expectedSupplier = updatedSupplier.DeepClone();
            Guid supplierId = inputSupplier.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(supplierId))
                    .ReturnsAsync(storageSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSupplierAsync(inputSupplier))
                    .ReturnsAsync(updatedSupplier);

            // when
            Supplier actualSupplier =
                await this.supplierService.ModifySupplierAsync(inputSupplier);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(inputSupplier.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(inputSupplier),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}