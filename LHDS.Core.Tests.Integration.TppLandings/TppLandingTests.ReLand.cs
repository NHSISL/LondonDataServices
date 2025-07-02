// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Integration.TppLandings
{
    public partial class TppLandingTests
    {
        [Fact]
        public async Task ShouldReLandTPPFileAsync()
        {
            // given
            Supplier supplier = await GetTppSupplier();

            // when, then
            await this.tppLandingClient.ReProcessAsync(
                supplierId: supplier.Id);
        }
    }
}
