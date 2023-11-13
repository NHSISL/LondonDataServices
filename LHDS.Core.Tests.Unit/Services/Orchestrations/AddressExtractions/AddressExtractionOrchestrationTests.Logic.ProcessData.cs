// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractionOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessZipFileWithOnlyCsvAddressesDataAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithOnlyCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            List<Address> randomAddresses = CreateRandomAddresses().ToList();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            List<Address> outputAddresses = await SetupMocksForProvidedZips(inputData, randomAddresses);
            List<Address> expectedAddresses = outputAddresses.DeepClone();

            // When
            List<Address> actualAddresses = await this.addressExtractionOrchestrationService
                .ProcessDataAsync(inputData);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            await VerifyMocksForProvidedZips(randomDateTimeOffset, randomId, inputData);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessZipFileWithZippedCsvAddressesDataAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessZipFileWithZippedCsvAddressesData.zip");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            List<Address> randomAddresses = CreateRandomAddresses().ToList();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            List<Address> outputAddresses = await SetupMocksForProvidedZips(inputData, randomAddresses);
            List<Address> expectedAddresses = outputAddresses.DeepClone();

            // When
            List<Address> actualAddresses = await this.addressExtractionOrchestrationService
                .ProcessDataAsync(inputData);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            await VerifyMocksForProvidedZips(randomDateTimeOffset, randomId, inputData);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        private async ValueTask<List<Address>> SetupMocksForProvidedZips(
            byte[] inputData,
            List<Address> randomAddresses)
        {
            List<Address> addresses = new List<Address>();

            using (MemoryStream memoryStream = new MemoryStream(inputData))
            {
                using (ZipArchive archive = new ZipArchive(memoryStream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var entryStream = entry.Open())
                            using (var tempMemoryStream = new MemoryStream())
                            {
                                addresses.AddRange(randomAddresses);
                                await entryStream.CopyToAsync(tempMemoryStream);
                                byte[] csvData = tempMemoryStream.ToArray();

                                this.addressParserServiceMock.Setup(service =>
                                    service.ProcessCsvAsync(csvData))
                                        .ReturnsAsync(randomAddresses);
                            }
                        }
                        else if (entry.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] zipFile;

                            using (MemoryStream nestedMemoryStream = new MemoryStream())
                            using (Stream entryStream = entry.Open())
                            {
                                entryStream.CopyTo(nestedMemoryStream);
                                zipFile = nestedMemoryStream.ToArray();
                            }

                            List<Address> childAddresses = await SetupMocksForProvidedZips(zipFile, randomAddresses);
                            addresses.AddRange(childAddresses);
                        }
                    }
                }
            }

            return addresses;
        }

        private async ValueTask VerifyMocksForProvidedZips(
            DateTimeOffset randomDateTimeOffset,
            Guid randomId,
            byte[] inputData)
        {
            using (MemoryStream memoryStream = new MemoryStream(inputData))
            {
                using (ZipArchive archive = new ZipArchive(memoryStream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var entryStream = entry.Open())
                            using (var tempMemoryStream = new MemoryStream())
                            {
                                await entryStream.CopyToAsync(tempMemoryStream);
                                byte[] csvData = tempMemoryStream.ToArray();

                                this.addressParserServiceMock.Verify(service =>
                                    service.ProcessCsvAsync(csvData),
                                        Times.Once);
                            }

                            var audit = new AddressExtractionAudit
                            {
                                Id = randomId,
                                CorrelationId = randomId,
                                FileName = $"{entry}",
                                Message = "Success",
                                MessageId = "",
                                CreatedBy = "System",
                                UpdatedBy = "System",
                                UpdatedDate = randomDateTimeOffset,
                                CreatedDate = randomDateTimeOffset,
                            };

                            this.addressExtractionAuditServiceMock.Verify(service =>
                                service.AddAddressExtractionAuditAsync(It.Is(SameAddressExtractionAuditAs(audit))),
                                    Times.Once);
                        }
                        else if (entry.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] zipFile;

                            using (MemoryStream nestedMemoryStream = new MemoryStream())
                            using (Stream entryStream = entry.Open())
                            {
                                entryStream.CopyTo(nestedMemoryStream);
                                zipFile = nestedMemoryStream.ToArray();
                            }

                            await VerifyMocksForProvidedZips(randomDateTimeOffset, randomId, zipFile);
                        }
                    }

                    this.dateTimeBrokerMock.Verify(broker =>
                        broker.GetCurrentDateTimeOffset(),
                            Times.Exactly(archive.Entries.Count));
                }
            }
        }
    }
}

