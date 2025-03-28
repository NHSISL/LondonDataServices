// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowAggregateDependencyValidationExceptionOnAddressIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            List<Exception> exceptions = new List<Exception>();
            string assembly = Assembly.GetExecutingAssembly().Location;
            string zipFileName = "ShouldProcessZipFileWithZippedCsvAddressesData.zip";
            string csvFileName = "ShouldProcessZipFileWithOnlyCsvAddressesData.zip";

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{zipFileName}");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            string csvFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{csvFileName}");

            byte[] csvData = await File.ReadAllBytesAsync(csvFilePath);
            string stringData = Encoding.UTF8.GetString(csvData);
            List<string> records = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records.Where(record =>
               record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { "UPRN", 3 },
                    { "UPSN", 4 },
                    { "OrganisationName", 5 },
                    { "DepartmentName", 6 },
                    { "SubBuildingName", 7 },
                    { "BuildingName", 8 },
                    { "BuildingNumber", 9 },
                    { "DependentThoroughfare", 10 },
                    { "Thoroughfare", 11 },
                    { "DoubleDependentLocality", 12 },
                    { "DependentLocality", 13 },
                    { "PostTown", 14 },
                    { "PostCode", 15 }
                };

            Guid randomId = Guid.NewGuid();
            List<Address> randomAddresses = CreateRandomAddresses(count: GetRandomNumber()).ToList();//change to random number
            List<Address> outputAddresses = randomAddresses.DeepClone();
            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            string inputFileName = zipFileName;
            string randomTempPath = Path.GetTempPath();
            string ordinanceTempFolder = Path.Combine(randomTempPath, "OrdinanceData");

            string ordinanceTempCsvFile =
                Path.Combine(ordinanceTempFolder, "ShouldProcessZipFileWithOnlyCsvAddressesData.csv");

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ReturnsAsync(randomTempPath);

            this.fileBrokerMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder))
                    .ReturnsAsync(false);

            List<string> csvFiles = new List<string> { ordinanceTempCsvFile };

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"))
                    .ReturnsAsync(csvFiles);

            foreach (var csvFile in csvFiles)
            {
                this.fileBrokerMock.Setup(service =>
                    service.ReadFileAsync(csvFile))
                        .ThrowsAsync(dependencyValidationException);

                var addressOrchestrationDependencyValidationException =
                    new AddressOrchestrationDependencyValidationException(
                        message: "Address orchestration dependency validation error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                addressOrchestrationDependencyValidationException.AddData("ExtractionError", csvFile);
                exceptions.Add(addressOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to extract {exceptions.Count} address files. " +
                        "File has been moved to the error folder.",
                    exceptions);

            var failedAddressOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: failedAddressOrchestrationServiceException);

            // When
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: zipFileName);

            AddressOrchestrationServiceException actualAddressOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressOrchestrationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressOrchestrationServiceException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                    Times.Once());

            this.fileBrokerMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder),
                    Times.Once());

            this.fileBrokerMock.Verify(service =>
                service.CreateDirectoryAsync(ordinanceTempFolder),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"),
                    Times.Once());

            foreach (var csvFile in csvFiles)
            {
                this.fileBrokerMock.Verify(service =>
                    service.ReadFileAsync(csvFile),
                        Times.Once());

                var addressOrchestrationDependencyValidationLoggingException =
                    new AddressOrchestrationDependencyValidationException(
                        message: "Address orchestration dependency validation error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                addressOrchestrationDependencyValidationLoggingException.AddData("ExtractionError", csvFile);

                this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        addressOrchestrationDependencyValidationLoggingException))),
                            Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualAddressOrchestrationServiceException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnAddressIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            List<Exception> exceptions = new List<Exception>();
            string assembly = Assembly.GetExecutingAssembly().Location;
            string zipFileName = "ShouldProcessZipFileWithZippedCsvAddressesData.zip";
            string csvFileName = "ShouldProcessZipFileWithOnlyCsvAddressesData.zip";

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{zipFileName}");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            string csvFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{csvFileName}");

            byte[] csvData = await File.ReadAllBytesAsync(csvFilePath);
            string stringData = Encoding.UTF8.GetString(csvData);
            List<string> records = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records.Where(record =>
               record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { "UPRN", 3 },
                    { "UPSN", 4 },
                    { "OrganisationName", 5 },
                    { "DepartmentName", 6 },
                    { "SubBuildingName", 7 },
                    { "BuildingName", 8 },
                    { "BuildingNumber", 9 },
                    { "DependentThoroughfare", 10 },
                    { "Thoroughfare", 11 },
                    { "DoubleDependentLocality", 12 },
                    { "DependentLocality", 13 },
                    { "PostTown", 14 },
                    { "PostCode", 15 }
                };

            Guid randomId = Guid.NewGuid();
            List<Address> randomAddresses = CreateRandomAddresses(count: GetRandomNumber()).ToList();//change to random number
            List<Address> outputAddresses = randomAddresses.DeepClone();
            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            string inputFileName = zipFileName;
            string randomTempPath = Path.GetTempPath();
            string ordinanceTempFolder = Path.Combine(randomTempPath, "OrdinanceData");

            string ordinanceTempCsvFile =
                Path.Combine(ordinanceTempFolder, "ShouldProcessZipFileWithOnlyCsvAddressesData.csv");

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ReturnsAsync(randomTempPath);

            this.fileBrokerMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder))
                    .ReturnsAsync(false);

            List<string> csvFiles = new List<string> { ordinanceTempCsvFile };

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"))
                    .ReturnsAsync(csvFiles);

            foreach (var csvFile in csvFiles)
            {
                this.fileBrokerMock.Setup(service =>
                    service.ReadFileAsync(csvFile))
                        .ThrowsAsync(dependencyException);

                var addressOrchestrationDependencyException =
                    new AddressOrchestrationDependencyException(
                        message: "Address orchestration dependency error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyException.InnerException as Xeption);

                addressOrchestrationDependencyException.AddData("ExtractionError", csvFile);
                exceptions.Add(addressOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to extract {exceptions.Count} address files. " +
                        "File has been moved to the error folder.",
                    exceptions);

            var failedAddressOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: failedAddressOrchestrationServiceException);

            // When
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: zipFileName);

            AddressOrchestrationServiceException actualAddressOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressOrchestrationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressOrchestrationServiceException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                    Times.Once());

            this.fileBrokerMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder),
                    Times.Once());

            this.fileBrokerMock.Verify(service =>
                service.CreateDirectoryAsync(ordinanceTempFolder),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"),
                    Times.Once());

            foreach (var csvFile in csvFiles)
            {
                this.fileBrokerMock.Verify(service =>
                    service.ReadFileAsync(csvFile),
                        Times.Once());

                var addressOrchestrationDependencyLoggingException =
                    new AddressOrchestrationDependencyException(
                        message: "Address orchestration dependency error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyException.InnerException as Xeption);

                addressOrchestrationDependencyLoggingException.AddData("ExtractionError", csvFile);

                this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        addressOrchestrationDependencyLoggingException))),
                            Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualAddressOrchestrationServiceException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnAddressIfErrorsInLoopAndLogItAsync()
        {
            // Given
            List<Exception> exceptions = new List<Exception>();
            string assembly = Assembly.GetExecutingAssembly().Location;
            string zipFileName = "ShouldProcessZipFileWithZippedCsvAddressesData.zip";
            string csvFileName = "ShouldProcessZipFileWithOnlyCsvAddressesData.zip";
            var serviceException = new Exception();

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{zipFileName}");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            Stream inputStream = new MemoryStream(inputData);

            string csvFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                $"Resources/Services/Orchestrations/Addresses/{csvFileName}");

            byte[] csvData = await File.ReadAllBytesAsync(csvFilePath);
            string stringData = Encoding.UTF8.GetString(csvData);
            List<string> records = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records.Where(record =>
               record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { "UPRN", 3 },
                    { "UPSN", 4 },
                    { "OrganisationName", 5 },
                    { "DepartmentName", 6 },
                    { "SubBuildingName", 7 },
                    { "BuildingName", 8 },
                    { "BuildingNumber", 9 },
                    { "DependentThoroughfare", 10 },
                    { "Thoroughfare", 11 },
                    { "DoubleDependentLocality", 12 },
                    { "DependentLocality", 13 },
                    { "PostTown", 14 },
                    { "PostCode", 15 }
                };

            Guid randomId = Guid.NewGuid();
            List<Address> randomAddresses = CreateRandomAddresses(count: GetRandomNumber()).ToList();//change to random number
            List<Address> outputAddresses = randomAddresses.DeepClone();
            string stringRecords = string.Join(Environment.NewLine, filteredRecords);
            string inputFileName = zipFileName;
            string randomTempPath = Path.GetTempPath();
            string ordinanceTempFolder = Path.Combine(randomTempPath, "OrdinanceData");

            string ordinanceTempCsvFile =
                Path.Combine(ordinanceTempFolder, "ShouldProcessZipFileWithOnlyCsvAddressesData.csv");

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ReturnsAsync(randomTempPath);

            this.fileBrokerMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder))
                    .ReturnsAsync(false);

            List<string> csvFiles = new List<string> { ordinanceTempCsvFile };

            this.fileBrokerMock.Setup(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"))
                    .ReturnsAsync(csvFiles);

            var innerFailedAddressOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: innerFailedAddressOrchestrationServiceException);


            foreach (var csvFile in csvFiles)
            {
                this.fileBrokerMock.Setup(service =>
                    service.ReadFileAsync(csvFile))
                        .ThrowsAsync(serviceException);

                innerAddressOrchestrationServiceException.AddData("ExtractionError", csvFile);

                exceptions.Add(innerAddressOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    message: $"Unable to extract {exceptions.Count} address files. " +
                        "File has been moved to the error folder.",
                    exceptions);

            var failedAddressOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: failedAddressOrchestrationServiceException);

            // When
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: zipFileName);

            AddressOrchestrationServiceException actualAddressOrchestrationServiceException =
                await Assert.ThrowsAsync<AddressOrchestrationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualAddressOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedAddressOrchestrationServiceException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                    Times.Once());

            this.fileBrokerMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(ordinanceTempFolder),
                    Times.Once());

            this.fileBrokerMock.Verify(service =>
                service.CreateDirectoryAsync(ordinanceTempFolder),
                    Times.Once);

            this.fileBrokerMock.Verify(service =>
                service.GetListOfFilesAsync(ordinanceTempFolder, "*.csv"),
                    Times.Once());

            foreach (var csvFile in csvFiles)
            {
                this.fileBrokerMock.Verify(service =>
                    service.ReadFileAsync(csvFile),
                        Times.Once());

                this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        innerAddressOrchestrationServiceException))),
                            Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualAddressOrchestrationServiceException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(someFilename);
            Stream inputStream = new MemoryStream(inputData);

            var expectedDependencyException =
                new AddressOrchestrationDependencyValidationException(
                    message: "Address orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: someFilename);

            AddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(someFilename);
            Stream inputStream = new MemoryStream(inputData);

            var expectedDependencyException =
                new AddressOrchestrationDependencyException(
                    message: "Address orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: someFilename);

            AddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationDependencyException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddressIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(someFilename);
            Stream inputStream = new MemoryStream(inputData);
            var serviceException = new Exception();

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressOrchestrationServiceException(
                    message: "Failed address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            this.fileBrokerMock.Setup(service =>
                service.GetTempPath())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask processDataTask = this.addressOrchestrationService
                .BulkAddAddressesAsync(input: inputStream, fileName: someFilename);

            AddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressOrchestrationServiceException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressOrchestrationServiceException);

            this.fileBrokerMock.Verify(service =>
                service.GetTempPath(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAddressOrchestrationServiceException))),
                       Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

