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

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSupplierAsync(inputSupplier))
                    .ReturnsAsync(updatedSupplier);

            // when
            Supplier actualSupplier =
                await this.supplierService.ModifySupplierAsync(inputSupplier);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(inputSupplier),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}