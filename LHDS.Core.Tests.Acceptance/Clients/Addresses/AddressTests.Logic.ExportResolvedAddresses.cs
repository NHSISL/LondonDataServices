// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldExportResolvedAddressesAsync()
        {
            //Given
            DateTimeOffset randomDateTimeOffset = dateTimeBroker.GetCurrentDateTimeOffset();
            string fileName = GetRandomString();
            int count = GetRandomNumber();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 1);
            List<ResolvedAddress> expectedResolvedAddresses = new List<ResolvedAddress>();
            List<Address> addedAddresses = new List<Address>();
            Guid batchReferenceId = Guid.NewGuid();

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
            }

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReferenceId);

            //When
            await this.addressClient.ExportResolvedAddressesAsync();

            //Then
            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once());

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress retrievedResolvedAddress =
                    await this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

                retrievedResolvedAddress.BatchReference.Should().Be(batchReferenceId);
                retrievedResolvedAddress.IsProcessing.Should().Be(false);
                retrievedResolvedAddress.RetryCount.Should().Be(0);
                retrievedResolvedAddress.IsExported.Should().Be(true);
                retrievedResolvedAddress.IsProcessed.Should().Be(true);
            }

            Stream retrievedDocumentStream = new MemoryStream();
            string csvFileName = $"out/{batchReferenceId}.csv";
            await this.documentService.RetrieveDocumentByFileNameAsync(retrievedDocumentStream, csvFileName, "address");

            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}