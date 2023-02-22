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
        public async Task ShouldAddSupplierAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset);
            Supplier inputSupplier = randomSupplier;
            Supplier storageSupplier = inputSupplier;
            Supplier expectedSupplier = storageSupplier.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSupplierAsync(inputSupplier))
                    .ReturnsAsync(storageSupplier);

            // when
            Supplier actualSupplier = await this.supplierService
                .AddSupplierAsync(inputSupplier);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(inputSupplier),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}