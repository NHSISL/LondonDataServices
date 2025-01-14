// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldReturnSuppliersAsync()
        {
            // given
            IQueryable<Supplier> randomSuppliers = CreateRandomSuppliers();
            IQueryable<Supplier> storageSuppliers = randomSuppliers;
            IQueryable<Supplier> expectedSuppliers = storageSuppliers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSuppliersAsync())
                    .ReturnsAsync(storageSuppliers);

            // when
            IQueryable<Supplier> actualSuppliers =
                await this.supplierService.RetrieveAllSuppliersAsync();

            // then
            actualSuppliers.Should().BeEquivalentTo(expectedSuppliers);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliersAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}