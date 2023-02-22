using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public void ShouldReturnSuppliers()
        {
            // given
            IQueryable<Supplier> randomSuppliers = CreateRandomSuppliers();
            IQueryable<Supplier> storageSuppliers = randomSuppliers;
            IQueryable<Supplier> expectedSuppliers = storageSuppliers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSuppliers())
                    .Returns(storageSuppliers);

            // when
            IQueryable<Supplier> actualSuppliers =
                this.supplierService.RetrieveAllSuppliers();

            // then
            actualSuppliers.Should().BeEquivalentTo(expectedSuppliers);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliers(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}