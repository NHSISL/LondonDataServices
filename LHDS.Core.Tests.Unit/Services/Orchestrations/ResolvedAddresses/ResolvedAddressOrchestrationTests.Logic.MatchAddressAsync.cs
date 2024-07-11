// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task MatchAddressAsync()
        {
            ////Given
            //List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses();
            //List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;

            //string inputResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault().UnstructuredPostalAddress;

            //ValueTask<AssignAddress> randomAssignAddress = CreateRandomAssignAddress();
            //ValueTask<AssignAddress> storageAssignAddress = randomAssignAddress;

            //Address randomAddress = CreateRandomAddress();
            //Address storageAddress = randomAddress;

            //this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
            //   service.RetrieveAllResolvedAddresses())
            //       .Returns(unmatchedResolvedAddresses.AsQueryable())
            //       .Returns(new List<ResolvedAddress>().AsQueryable());

            ////modify resolved to processinbg = true
            //// retry count += 
            ////updatedate = now



            //this.assignProcessingServiceMock.Setup(processing =>
            //    processing.MatchAddressAsync(inputResolvedAddress))
            //        .Returns(storageAssignAddress);

            //this.addressProcessingServiceMock.Setup(processing =>
            //    processing.RetrieveAllAddresses())
            //        .Returns(storageAddress);


            //ResolvedAddress newResolvedAddress = unmatchedResolvedAddresses.DeepClone();
            ////newResolvedAddress.Matched = storageAssignAddress.
            ////newResolvedAddress.MatchPattern = storageAssignAddress.processing = 0;



            //this.resolvedAddressProcessingServiceMock.Setup(processing =>
            //   processing.ModifyResolvedAddressAsync(newResolvedAddress))
            //       .Returns(newResolvedAddress);

            ////When

            ////Then
        }
    }
}
