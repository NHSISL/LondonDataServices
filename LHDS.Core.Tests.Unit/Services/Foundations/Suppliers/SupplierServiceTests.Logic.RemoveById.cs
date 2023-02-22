using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSupplierByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputSupplierId = randomId;
            Supplier randomSupplier = CreateRandomSupplier();
            Supplier storageSupplier = randomSupplier;
            Supplier expectedInputSupplier = storageSupplier;
            Supplier deletedSupplier = expectedInputSupplier;
            Supplier expectedSupplier = deletedSupplier.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(inputSupplierId))
                    .ReturnsAsync(storageSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSupplierAsync(expectedInputSupplier))
                    .ReturnsAsync(deletedSupplier);

            // when
            Supplier actualSupplier = await this.supplierService
                .RemoveSupplierByIdAsync(inputSupplierId);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(inputSupplierId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSupplierAsync(expectedInputSupplier),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}