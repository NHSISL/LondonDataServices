// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldRetrieveSupplierByIdAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            Supplier inputSupplier = randomSupplier;
            Supplier storageSupplier = randomSupplier;
            Supplier expectedSupplier = storageSupplier.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(inputSupplier.Id))
                    .ReturnsAsync(storageSupplier);

            // when
            Supplier actualSupplier =
                await this.supplierService.RetrieveSupplierByIdAsync(inputSupplier.Id);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(inputSupplier.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}