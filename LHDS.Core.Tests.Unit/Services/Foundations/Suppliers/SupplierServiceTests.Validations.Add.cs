using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSupplierIsNullAndLogItAsync()
        {
            // given
            Supplier nullSupplier = null;

            var nullSupplierException =
                new NullSupplierException();

            var expectedSupplierValidationException =
                new SupplierValidationException(nullSupplierException);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(nullSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should().BeEquivalentTo(expectedSupplierValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}