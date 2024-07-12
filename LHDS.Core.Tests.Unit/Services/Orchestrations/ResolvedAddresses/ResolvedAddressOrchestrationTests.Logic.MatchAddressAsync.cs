// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task MatchAddressAsync()
        {
            //Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses();
            List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;
            string inputResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault().UnstructuredPostalAddress;

            ValueTask<AssignAddress> randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);
            ValueTask<AssignAddress> storageAssignAddress = randomAssignAddress;
            AssignAddress assignAddress = await randomAssignAddress;
            string matchedUprn = assignAddress.UPRN.ToString();

            ValueTask<Address?> randomAddress = CreateRandomAddress(randomDateTimeOffset);
            ValueTask<Address?> storageAddress = randomAddress;
            Address? ordananceAddress = await storageAddress;

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddresses())
                   .Returns(unmatchedResolvedAddresses.AsQueryable())
                   .Returns(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            ResolvedAddress processingResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault();
            processingResolvedAddress.IsProcessing = true;
            processingResolvedAddress.RetryCount += 1;
            processingResolvedAddress.UpdatedDate = randomDateTimeOffset;

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyResolvedAddressAsync(processingResolvedAddress))
                    .ReturnsAsync(processingResolvedAddress);

            this.assignProcessingServiceMock.Setup(processing =>
                processing.MatchAddressAsync(inputResolvedAddress))
                    .Returns(storageAssignAddress);

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressByUPRNAsync(matchedUprn))
                    .Returns(storageAddress);


            ResolvedAddress newResolvedAddress = processingResolvedAddress;
            newResolvedAddress.AddressFormatQuality = assignAddress.AddressFormat;
            newResolvedAddress.PostCodeQuality = assignAddress.PostcodeQuality;
            newResolvedAddress.Matched = assignAddress.Matched);
            newResolvedAddress.Qualifier = assignAddress.Qualifier;
            newResolvedAddress.Classification = assignAddress.Classification;
            newResolvedAddress.Algorithm = assignAddress.Algorithm;
            newResolvedAddress.MatchPattern = assignAddress.MatchPattern.ToString();

            newResolvedAddress.UPSN = ordananceAddress.UPSN;
            newResolvedAddress.Thoroughfare

            newResolvedAddress.IsProcessing = false;





            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.ModifyResolvedAddressAsync(newResolvedAddress))
                   .ReturnsAsync(newResolvedAddress);

            //When

            //Then
        }
    }
}
