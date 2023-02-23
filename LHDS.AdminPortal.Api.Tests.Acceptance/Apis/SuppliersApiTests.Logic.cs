using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Suppliers
{
    public partial class SuppliersApiTests
    {
        [Fact]
        public async Task ShouldPostSupplierAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            Supplier inputSupplier = randomSupplier;
            Supplier expectedSupplier = inputSupplier;

            // when 
            await this.apiBroker.PostSupplierAsync(inputSupplier);

            Supplier actualSupplier =
                await this.apiBroker.GetSupplierByIdAsync(inputSupplier.Id);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);
            await this.apiBroker.DeleteSupplierByIdAsync(actualSupplier.Id);
        }
    }
}