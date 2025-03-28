// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldMatchAddressDataAsync()
        {
            //Given
            IQueryable<ResolvedAddress> retrievedResolvedAddresses =
                    await this.resolvedAddressService.RetrieveAllResolvedAddressesAsync();

            IQueryable<ResolvedAddress> unprocessedResolvedAddresses = retrievedResolvedAddresses
                .Where(address => address.IsProcessed == false);

            //When
            await this.addressClient.MatchAddressDataAsync();

            //Then
            IQueryable<ResolvedAddress> resultantRetrievedResolvedAddresses =
                    await this.resolvedAddressService.RetrieveAllResolvedAddressesAsync();

            IQueryable<ResolvedAddress> processedResolvedAddress = resultantRetrievedResolvedAddresses
                .Where(address => address.IsProcessed == true);

            List<ResolvedAddress> unmatchedResolvedAddress =
                unprocessedResolvedAddresses.Intersect(processedResolvedAddress).ToList();
        }
    }
}
