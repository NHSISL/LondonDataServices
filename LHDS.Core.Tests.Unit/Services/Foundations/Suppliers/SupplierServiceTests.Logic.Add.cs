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
        public async Task ShouldAddSupplierAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset);
            Supplier inputSupplier = randomSupplier;
            Supplier storageSupplier = inputSupplier;
            Supplier expectedSupplier = storageSupplier.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectSupplierByIdAsync(inputSupplier.Id))
                    .ReturnsAsync(storageSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSupplierAsync(inputSupplier))
                    .ReturnsAsync(storageSupplier);

            // when
            Supplier actualSupplier = await this.supplierService
                .AddSupplierAsync(inputSupplier);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
               broker.SelectSupplierByIdAsync(inputSupplier.Id),
                   Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(inputSupplier),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}