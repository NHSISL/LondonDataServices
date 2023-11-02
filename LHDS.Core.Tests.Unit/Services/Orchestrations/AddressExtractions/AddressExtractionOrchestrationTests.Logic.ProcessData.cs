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
            try
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

                this.addressParserServiceMock.VerifyNoOtherCalls();
                this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
                this.dateTimeBrokerMock.VerifyNoOtherCalls();
                this.loggingBrokerMock.VerifyNoOtherCalls();
            }
            catch (Exception ex)
            {
                var message = $"Error: {ex.Message}, " +
                    $"Inner ex: {ex.InnerException.Message}, " +
                    $"Inner inner ex: {ex.InnerException.InnerException.Message}";

                output.WriteLine(message);
                Assert.Fail(message);
            }
        }

        [Fact]
        public async Task ShouldProcessZipFileWithZippedCsvAddressesDataAndLogAsync()
        {
            try
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

                this.addressParserServiceMock.VerifyNoOtherCalls();
                this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
                this.dateTimeBrokerMock.VerifyNoOtherCalls();
                this.loggingBrokerMock.VerifyNoOtherCalls();
            }
            catch (Exception ex)
            {
                var message = $"Error: {ex.Message}, " +
                    $"Inner ex: {ex.InnerException.Message}, " +
                    $"Inner inner ex: {ex.InnerException.InnerException.Message}";

                output.WriteLine(message);
                Assert.Fail(message);
            }
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




        //[Fact]
        //public async Task ShouldProcessAddressesDataAndLogAsync()
        //{
        //    List<Address> expectedAddresses = null;  // Declare outside the try block
        //    List<Address> actualAddresses = null;
        //    try
        //    {
        //        // Given
        //        string assembly = Assembly.GetExecutingAssembly().Location;
        //        string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestNestedZip.zip");
        //        byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

        //        List<string> unZippedInputFilePaths =
        //            new List<string> {
        //            Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestCsv2.csv"),
        //            Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestCsv1.csv")
        //            };

        //        List<Address> outputAddresses = new List<Address>();

        //        foreach (string filePath in unZippedInputFilePaths)
        //        {
        //            List<Address> randomAddresses = CreateRandomAddresses().ToList();
        //            outputAddresses.AddRange(randomAddresses);
        //            byte[] csvData = await File.ReadAllBytesAsync(filePath);

        //            this.addressParserServiceMock.Setup(service =>
        //                service.ProcessCsvAsync(csvData))
        //                    .ReturnsAsync(randomAddresses);
        //        }

        //        expectedAddresses = outputAddresses.DeepClone();

        //        // When
        //        actualAddresses =
        //            await this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

        //        // Then
        //        var x = 1;
        //        actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
        //           options.Excluding(address => address.Id));

        //        this.addressParserServiceMock.Verify(service =>
        //            service.ProcessCsvAsync(It.IsAny<byte[]>()),
        //                Times.Exactly(unZippedInputFilePaths.Count()));

        //        this.dateTimeBrokerMock.Verify(broker =>
        //            broker.GetCurrentDateTimeOffset(),
        //                Times.Exactly(unZippedInputFilePaths.Count() * 2));

        //        this.addressExtractionAuditServiceMock.Verify(service =>
        //            service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
        //                Times.Exactly(unZippedInputFilePaths.Count));

        //        this.addressParserServiceMock.VerifyNoOtherCalls();
        //        this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
        //        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        //        this.loggingBrokerMock.VerifyNoOtherCalls();
        //    }
        //    catch (Exception ex)
        //    {
        //        var actualAddressesStr = actualAddresses != null
        //            ? string.Join(", ", actualAddresses.Select(a => a.ToString()))
        //            : "Not set";

        //        var message = $"Error: {ex.Message}, Inner ex: {ex.InnerException.Message}, Inner inner ex: {ex.InnerException.InnerException.Message}, actualAddress: {actualAddressesStr}";
        //        output.WriteLine(message);
        //        Assert.Fail(message);
        //    }
        //}

        //[Fact]
        //public async Task ShouldProcessZipFileWithCsvAddressesDataAndLogAsync()
        //{
        //    List<Address> expectedAddresses = null;  // Declare outside the try block
        //    List<Address> actualAddresses = null;
        //    try
        //    {
        //        // Given
        //        string assembly = Assembly.GetExecutingAssembly().Location;
        //        string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestCsv2.zip");
        //        byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

        //        List<string> unZippedInputFilePaths =
        //            new List<string> {
        //        Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestCsv2.csv")
        //            };

        //        List<Address> outputAddresses = new List<Address>();

        //        foreach (string filePath in unZippedInputFilePaths)
        //        {
        //            List<Address> randomAddresses = CreateRandomAddresses().ToList();
        //            outputAddresses.AddRange(randomAddresses);
        //            byte[] csvData = await File.ReadAllBytesAsync(filePath);

        //            this.addressParserServiceMock.Setup(service =>
        //                service.ProcessCsvAsync(csvData))
        //                    .ReturnsAsync(randomAddresses);
        //        }

        //        expectedAddresses = outputAddresses.DeepClone();

        //        // When
        //        actualAddresses =
        //            await this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

        //        // Then
        //        var x = 1;
        //        actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
        //            options.Excluding(address => address.Id));

        //        this.addressParserServiceMock.Verify(service =>
        //            service.ProcessCsvAsync(It.IsAny<byte[]>()),
        //                Times.Exactly(unZippedInputFilePaths.Count()));

        //        this.dateTimeBrokerMock.Verify(broker =>
        //            broker.GetCurrentDateTimeOffset(),
        //                Times.Exactly(unZippedInputFilePaths.Count() * 2));

        //        this.addressExtractionAuditServiceMock.Verify(service =>
        //            service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
        //                Times.Exactly(unZippedInputFilePaths.Count));

        //        this.addressParserServiceMock.VerifyNoOtherCalls();
        //        this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
        //        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        //        this.loggingBrokerMock.VerifyNoOtherCalls();
        //    }
        //    catch (Exception ex)
        //    {
        //        var actualAddressesStr = actualAddresses != null
        //            ? string.Join(", ", actualAddresses.Select(a => a.ToString()))
        //            : "Not set";

        //        var message = $"Error: {ex.Message}, Inner ex: {ex.InnerException.Message}, Inner inner ex: {ex.InnerException.InnerException.Message}, actualAddress: {actualAddressesStr}";
        //        output.WriteLine(message);
        //        Assert.Fail(message);
        //    }
        //}

        //[Fact]
        //public async Task ShouldProcessZipFileWithNestedZipAddressesDataAndLogAsync()
        //{
        //    List<Address> expectedAddresses = null;  // Declare outside the try block
        //    List<Address> actualAddresses = null;
        //    try
        //    {
        //        // Given
        //        string assembly = Assembly.GetExecutingAssembly().Location;
        //        string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly), @"Resources/NestedCsvZip.zip");
        //        byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

        //        List<string> unZippedInputFilePaths =
        //            new List<string> {
        //        Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestCsv2.csv")
        //            };

        //        List<Address> outputAddresses = new List<Address>();

        //        foreach (string filePath in unZippedInputFilePaths)
        //        {
        //            List<Address> randomAddresses = CreateRandomAddresses().ToList();
        //            outputAddresses.AddRange(randomAddresses);
        //            byte[] csvData = await File.ReadAllBytesAsync(filePath);

        //            this.addressParserServiceMock.Setup(service =>
        //                service.ProcessCsvAsync(csvData))
        //                    .ReturnsAsync(randomAddresses);
        //        }

        //        expectedAddresses = outputAddresses.DeepClone();

        //        // When
        //        actualAddresses =
        //            await this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

        //        // Then
        //        var x = 1;
        //        actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
        //            options.Excluding(address => address.Id));

        //        this.addressParserServiceMock.Verify(service =>
        //            service.ProcessCsvAsync(It.IsAny<byte[]>()),
        //                Times.Exactly(unZippedInputFilePaths.Count()));

        //        this.dateTimeBrokerMock.Verify(broker =>
        //            broker.GetCurrentDateTimeOffset(),
        //                Times.Exactly(unZippedInputFilePaths.Count() * 2));

        //        this.addressExtractionAuditServiceMock.Verify(service =>
        //            service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
        //                Times.Exactly(unZippedInputFilePaths.Count));

        //        this.addressParserServiceMock.VerifyNoOtherCalls();
        //        this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
        //        this.dateTimeBrokerMock.VerifyNoOtherCalls();
        //        this.loggingBrokerMock.VerifyNoOtherCalls();
        //    }
        //    catch (Exception ex)
        //    {
        //        var actualAddressesStr = actualAddresses != null
        //            ? string.Join(", ", actualAddresses.Select(a => a.ToString()))
        //            : "Not set";

        //        var message = $"Error: {ex.Message}, Inner ex: {ex.InnerException.Message}, Inner inner ex: {ex.InnerException.InnerException.Message}, actualAddress: {actualAddressesStr}";
        //        output.WriteLine(message);
        //        Assert.Fail(message);
        //    }
        //}
    }
}

