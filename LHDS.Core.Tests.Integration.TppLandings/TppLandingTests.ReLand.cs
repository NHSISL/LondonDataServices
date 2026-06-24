// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers;

namespace LHDS.Core.Tests.Integration.TppLandings
{
    public partial class TppLandingTests
    {
        [ReleaseCandidateFact]
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
