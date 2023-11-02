// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessAddressesDataAndLogAsync()
        {
            List<Address> expectedAddresses = null;  // Declare outside the try block
            List<Address> actualAddresses = null;
            try
            {
                // Given
                string assembly = Assembly.GetExecutingAssembly().Location;
                string inputFilePath = Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestNestedZip.zip");
                byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

                List<string> unZippedInputFilePaths =
                    new List<string> {
                    Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestCsv2.csv"),
                    Path.Combine(Path.GetDirectoryName(assembly), @"Resources/TestCsv1.csv")
                    };

                List<Address> outputAddresses = new List<Address>();

                foreach (string filePath in unZippedInputFilePaths)
                {
                    List<Address> randomAddresses = CreateRandomAddresses().ToList();
                    outputAddresses.AddRange(randomAddresses);
                    byte[] csvData = await File.ReadAllBytesAsync(filePath);

                    this.addressParserServiceMock.Setup(service =>
                        service.ProcessCsvAsync(csvData))
                            .ReturnsAsync(randomAddresses);
                }

                expectedAddresses = outputAddresses.DeepClone();

                // When
                actualAddresses =
                    await this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

                // Then
                var x = 1;
                actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                   options.Excluding(address => address.Id));

                this.addressParserServiceMock.Verify(service =>
                    service.ProcessCsvAsync(It.IsAny<byte[]>()),
                        Times.Exactly(unZippedInputFilePaths.Count()));

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(unZippedInputFilePaths.Count() * 2));

                this.addressExtractionAuditServiceMock.Verify(service =>
                    service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                        Times.Exactly(unZippedInputFilePaths.Count));

                this.addressParserServiceMock.VerifyNoOtherCalls();
                this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
                this.dateTimeBrokerMock.VerifyNoOtherCalls();
                this.loggingBrokerMock.VerifyNoOtherCalls();
            }
            catch (Exception ex)
            {
                var actualAddressesStr = actualAddresses != null
                    ? string.Join(", ", actualAddresses.Select(a => a.ToString()))
                    : "Not set";

                var message = $"Error: {ex.Message}, Inner ex: {ex.InnerException.Message}, Inner inner ex: {ex.InnerException.InnerException.Message}, actualAddress: {actualAddressesStr}";
                output.WriteLine(message);
                Assert.Fail(message);
            }
        }
    }
}

