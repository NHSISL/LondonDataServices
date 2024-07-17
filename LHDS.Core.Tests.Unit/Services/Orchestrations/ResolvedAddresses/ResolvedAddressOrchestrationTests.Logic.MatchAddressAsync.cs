// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
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
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: GetRandomNumber());
            List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;

            string inputResolvedAddress = unmatchedResolvedAddresses
                .FirstOrDefault().UnstructuredPostalAddress;

            AssignAddress randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);
            AssignAddress storageAssignAddress = randomAssignAddress;
            string matchedUprn = storageAssignAddress.UPRN.ToString();

            Address? randomAddress = CreateRandomAddress(randomDateTimeOffset);
            Address? storageAddress = randomAddress;
            Address? ordananceAddress = storageAddress;

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddresses())
                   .Returns(unmatchedResolvedAddresses.AsQueryable())
                   .Returns(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            ResolvedAddress processingResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault().DeepClone();
            ResolvedAddress lockedResolvedAddress = processingResolvedAddress;
            lockedResolvedAddress.IsProcessing = true;
            lockedResolvedAddress.RetryCount += 1;
            lockedResolvedAddress.UpdatedDate = randomDateTimeOffset;
            ResolvedAddress foundAddress = lockedResolvedAddress.DeepClone();
            ResolvedAddress foundAddressLocked = foundAddress.DeepClone();

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(lockedResolvedAddress))))
                    .ReturnsAsync(foundAddress);

            this.assignProcessingServiceMock.Setup(processing =>
                processing.MatchAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(storageAssignAddress);

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressByUPRNAsync(matchedUprn))
                    .ReturnsAsync(storageAddress);

            ResolvedAddress newResolvedAddress =
                MapOrdananceWithAssign(
                    foundAddressLocked,
                    storageAssignAddress,
                    ordananceAddress);

            newResolvedAddress.UpdatedDate = randomDateTimeOffset;

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.ModifyResolvedAddressAsync(newResolvedAddress))
                   .ReturnsAsync(newResolvedAddress);

            //When
            await this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            //Then
            this.resolvedAddressProcessingServiceMock.Verify(service =>
               service.RetrieveAllResolvedAddresses(),
                   Times.Exactly(2));

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(),
                   Times.Exactly(2));

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                 processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(lockedResolvedAddress))),
                     Times.Once());

            //It.Is(SameResolvedAddressAs(

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
               processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(newResolvedAddress))),
                   Times.Once());

            this.assignProcessingServiceMock.Verify(processing =>
                processing.MatchAddressAsync(inputResolvedAddress),
                    Times.Once());

            this.addressProcessingServiceMock.Verify(processing =>
                processing.RetrieveAddressByUPRNAsync(matchedUprn),
                    Times.Once());

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        private static ResolvedAddress MapOrdananceWithAssign(
            ResolvedAddress processingResolvedAddress,
            AssignAddress assignAddress,
            Address? ordananceAddress)
        {
            ResolvedAddress newResolvedAddress = processingResolvedAddress;
            newResolvedAddress.UPSN = ordananceAddress.UPSN ?? null;
            newResolvedAddress.OrganisationName = ordananceAddress.OrganisationName;
            newResolvedAddress.DepartmentName = ordananceAddress.DepartmentName;
            newResolvedAddress.SubBuildingName = ordananceAddress.SubBuildingName;
            newResolvedAddress.BuildingName = ordananceAddress.BuildingName;
            newResolvedAddress.BuildingNumber = ordananceAddress.BuildingNumber;
            newResolvedAddress.DependentThoroughfare = ordananceAddress.DependentThoroughfare;
            newResolvedAddress.Thoroughfare = ordananceAddress.Thoroughfare;
            newResolvedAddress.DoubleDependentLocality = ordananceAddress.DoubleDependentLocality;
            newResolvedAddress.DependentLocality = ordananceAddress.DependentLocality;
            newResolvedAddress.PostTown = ordananceAddress.PostTown;
            newResolvedAddress.PostCode = ordananceAddress.PostCode;
            newResolvedAddress.AddressFormatQuality = assignAddress.AddressFormat;
            newResolvedAddress.PostCodeQuality = assignAddress.PostcodeQuality;
            newResolvedAddress.Matched = assignAddress.Matched;
            newResolvedAddress.Qualifier = assignAddress.Qualifier;
            newResolvedAddress.Classification = assignAddress.Classification;
            newResolvedAddress.Algorithm = assignAddress.Algorithm;
            newResolvedAddress.MatchPattern = assignAddress.MatchPattern.ToString();
            newResolvedAddress.IsProcessing = false;
            newResolvedAddress.IsExported = false;
            newResolvedAddress.RetryCount = 0;

            return newResolvedAddress;
        }
    }
}
