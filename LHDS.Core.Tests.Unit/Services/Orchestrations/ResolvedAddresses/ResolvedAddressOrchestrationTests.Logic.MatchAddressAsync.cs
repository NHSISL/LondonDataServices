// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Audits;
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
            Guid identifier = Guid.NewGuid();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: GetRandomNumber());
            List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;
            string inputResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault().UnstructuredPostalAddress;
            AssignAddress randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);
            AssignAddress storageAssignAddress = randomAssignAddress;
            string matchedUprn = storageAssignAddress.BestMatch.UPRN.ToString();
            Address randomAddress = CreateRandomAddress(randomDateTimeOffset);
            Address storageAddress = randomAddress;
            Address ordananceAddress = storageAddress;

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(identifier);

            this.securityBrokerMock.Setup(broker =>
               broker.GetCurrentUserAsync())
                   .ReturnsAsync(randomEntraUser);

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddressesAsync())
                   .ReturnsAsync(unmatchedResolvedAddresses.AsQueryable())
                   .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

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

            storageAddress.UPRN = matchedUprn;

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressByUPRNAsync(matchedUprn))
                    .ReturnsAsync(storageAddress);

            ResolvedAddress newResolvedAddress =
                MapOrdananceWithAssign(
                    foundAddressLocked,
                    storageAssignAddress,
                    ordananceAddress);

            newResolvedAddress.UpdatedDate = randomDateTimeOffset;
            newResolvedAddress.IsProcessed = true;
            newResolvedAddress.UPRN = matchedUprn;

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.ModifyResolvedAddressAsync(newResolvedAddress))
                   .ReturnsAsync(newResolvedAddress);

            //When
            await this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            //Then
            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(3));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(service =>
               service.RetrieveAllResolvedAddressesAsync(),
                   Times.Exactly(2));

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffsetAsync(),
                   Times.Exactly(4));

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                 processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(lockedResolvedAddress))),
                     Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
               processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(newResolvedAddress))),
                   Times.Once());

            this.assignProcessingServiceMock.Verify(processing =>
                processing.MatchAddressAsync(inputResolvedAddress),
                    Times.Once());

            this.addressProcessingServiceMock.Verify(processing =>
                processing.RetrieveAddressByUPRNAsync(matchedUprn),
                    Times.Once());

            this.auditBrokerMock.Verify(broker =>
                broker.BulkLogAsync(It.IsAny<List<Audit>>()),
                    Times.Once());

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task NullAssignMatchAddressAsync()
        {
            //Given
            Guid identifier = Guid.NewGuid();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: GetRandomNumber());
            List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;

            string inputResolvedAddress =
                unmatchedResolvedAddresses?.FirstOrDefault()?.UnstructuredPostalAddress
                    ?? string.Empty;

            AssignAddress randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);
            AssignAddress storageAssignAddress = randomAssignAddress;
            AssignAddress nullStorageAssignAddress = null;

            Address ordananceAddress = null;

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(identifier);

            this.securityBrokerMock.Setup(broker =>
               broker.GetCurrentUserAsync())
                   .ReturnsAsync(randomEntraUser);

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddressesAsync())
                   .ReturnsAsync(unmatchedResolvedAddresses.AsQueryable())
                   .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

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
                    .ReturnsAsync(nullStorageAssignAddress);

            ResolvedAddress resolvedAddress =
                MapOrdananceWithAssign(
                    foundAddressLocked,
                    nullStorageAssignAddress,
                    ordananceAddress);

            resolvedAddress.UpdatedDate = randomDateTimeOffset;
            resolvedAddress.IsProcessed = true;
            resolvedAddress.UPRN = null;

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
               processing.ModifyResolvedAddressAsync(resolvedAddress))
                   .ReturnsAsync(resolvedAddress);

            //When
            await this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            //Then
            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(3));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(service =>
               service.RetrieveAllResolvedAddressesAsync(),
                   Times.Exactly(2));

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffsetAsync(),
                   Times.Exactly(4));

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                 processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(lockedResolvedAddress))),
                     Times.Once());

            this.assignProcessingServiceMock.Verify(processing =>
                processing.MatchAddressAsync(inputResolvedAddress),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
               processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(resolvedAddress))),
                   Times.Once());

            this.auditBrokerMock.Verify(broker =>
                broker.BulkLogAsync(It.IsAny<List<Audit>>()),
                    Times.Once());

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        private static ResolvedAddress MapOrdananceWithAssign(
            ResolvedAddress processingResolvedAddress,
            AssignAddress assignAddress,
            Address ordananceAddress)
        {
            ResolvedAddress newResolvedAddress = processingResolvedAddress;
            newResolvedAddress.UPSN = ordananceAddress?.UPSN ?? null;
            newResolvedAddress.OrganisationName = ordananceAddress?.OrganisationName;
            newResolvedAddress.DepartmentName = ordananceAddress?.DepartmentName;
            newResolvedAddress.SubBuildingName = ordananceAddress?.SubBuildingName;
            newResolvedAddress.BuildingName = ordananceAddress?.BuildingName;
            newResolvedAddress.BuildingNumber = ordananceAddress?.BuildingNumber;
            newResolvedAddress.DependentThoroughfare = ordananceAddress?.DependentThoroughfare;
            newResolvedAddress.Thoroughfare = ordananceAddress?.Thoroughfare;
            newResolvedAddress.DoubleDependentLocality = ordananceAddress?.DoubleDependentLocality;
            newResolvedAddress.DependentLocality = ordananceAddress?.DependentLocality;
            newResolvedAddress.PostTown = ordananceAddress?.PostTown;
            newResolvedAddress.PostCode = ordananceAddress?.PostCode;
            newResolvedAddress.AddressFormatQuality = assignAddress?.AddressFormat;
            newResolvedAddress.PostCodeQuality = assignAddress?.PostcodeQuality;
            newResolvedAddress.MatchedWithAssign = assignAddress?.Matched ?? false;
            newResolvedAddress.Qualifier = assignAddress?.BestMatch.Qualifier;
            newResolvedAddress.Classification = assignAddress?.BestMatch.Classification;
            newResolvedAddress.Algorithm = assignAddress?.BestMatch.Algorithm;
            newResolvedAddress.MatchPattern = assignAddress?.Pattern;
            newResolvedAddress.XCoordinate = ordananceAddress?.XCoordinate;
            newResolvedAddress.YCoordinate = ordananceAddress?.YCoordinate;
            newResolvedAddress.Latitude = ordananceAddress?.Latitude;
            newResolvedAddress.Longitude = ordananceAddress?.Longitude;
            newResolvedAddress.IsProcessing = false;
            newResolvedAddress.IsExported = false;
            newResolvedAddress.RetryCount = 0;

            return newResolvedAddress;
        }
    }
}
