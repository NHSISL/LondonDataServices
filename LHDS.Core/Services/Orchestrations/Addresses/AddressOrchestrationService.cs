// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.Addresses;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.Assigns;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationService : IAddressOrchestrationService
    {
        private readonly IAddressProcessingService addressProcessingService;
        private readonly IAssignProcessingService assignProcessingService;
        private readonly IFileBroker fileBroker;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressOrchestrationService(
            IAddressProcessingService addressProcessingService,
            IAssignProcessingService assignProcessingService,
            IFileBroker fileBroker,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker)
        {
            this.addressProcessingService = addressProcessingService;
            this.assignProcessingService = assignProcessingService;
            this.fileBroker = fileBroker;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.auditBroker = auditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask BulkAddAddressesAsync(Stream input, string fileName) =>
            TryCatch(async () =>
            {
                ValidateDataOnBulkAddAddresses(input, fileName);
                string tempfolder = await this.fileBroker.GetTempPath();
                string ordinanceTempFolder = Path.Combine(tempfolder, "OrdinanceData");
                string ordinanceFilePath = Path.Combine(ordinanceTempFolder, fileName);
                bool folderExists = await this.fileBroker.CheckIfDirectoryExistsAsync(ordinanceTempFolder);

                if (!folderExists)
                {
                    await this.fileBroker.CreateDirectoryAsync(ordinanceTempFolder);
                }

                await UnZipAndExtractAsync(data: input, extractPath: ordinanceTempFolder);
                await ReadCsvDataAndBulkAddAddresses(ordinanceTempFolder);
                await this.fileBroker.DeleteDirectoryAsync(ordinanceTempFolder, true);
            });

        virtual internal async ValueTask UnZipAndExtractAsync(Stream data, string extractPath)
        {
            using (ZipArchive archive = new ZipArchive(data, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string entryPath = Path.Combine(extractPath, entry.FullName);

                    if (!Directory.Exists(extractPath))
                    {
                        Directory.CreateDirectory(extractPath);
                    }

                    if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        using (Stream entryStream = entry.Open())
                        using (FileStream tempZipFileStream =
                            new FileStream(entryPath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            await entryStream.CopyToAsync(tempZipFileStream);
                        }

                        using (FileStream nestedZipStream =
                            new FileStream(entryPath, FileMode.Open, FileAccess.Read))
                        {
                            string nestedExtractPath = Path.Combine(extractPath, Path.GetFileNameWithoutExtension(entry.FullName));

                            if (!Directory.Exists(nestedExtractPath))
                            {
                                Directory.CreateDirectory(nestedExtractPath);
                            }

                            await UnZipAndExtractAsync(nestedZipStream, nestedExtractPath);
                        }

                        File.Delete(entryPath);
                    }
                    else
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(entryPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(entryPath));
                        }

                        using (Stream entryStream = entry.Open())
                        using (FileStream fileStream = new FileStream(entryPath, FileMode.Create, FileAccess.Write))
                        {
                            await entryStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        virtual internal async Task ReadCsvDataAndBulkAddAddresses(string tempFolder)
        {
            List<string> csvFiles = await fileBroker.GetListOfFilesAsync(tempFolder, "*.csv");
            var exceptions = new List<Exception>();

            foreach (string csvFile in csvFiles)
            {
                try
                {
                    await TryCatch(async () =>
                    {
                        byte[] csvData = await fileBroker.ReadFileAsync(csvFile);

                        string stringData = Encoding.UTF8.GetString(csvData);

                        List<string> records = stringData.Split(
                            new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

                        List<string> filteredRecords = records.Where(record =>
                           record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

                        string stringRecords = string.Join(Environment.NewLine, filteredRecords);

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

                        List<Address> addresses = await this.csvHelperBroker
                            .MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord: false, fieldMappings);

                        await addressProcessingService.BulkAddAddressesAsync(addresses, csvFile);
                    });
                }
                catch (Exception ex)
                {
                    ((Xeption)ex).AddData("ExtractionError", csvFile);
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to extract {exceptions.Count} address files. " +
                    $"File has been moved to the error folder.",
                    exceptions);
            }
        }

        virtual internal async ValueTask<List<Address>> MapDPADataToAddressesAsync(string dpaCsvFile)
        {
            bool fileExists = await this.fileBroker.CheckIfFileExistsAsync(dpaCsvFile);

            if (!fileExists)
            {
                throw new InvalidFileAddressOrchestrationException(
                    message: $"The file {dpaCsvFile} could not be found.");
            }

            byte[] csvData = await fileBroker.ReadFileAsync(dpaCsvFile);
            string stringData = Encoding.UTF8.GetString(csvData);

            List<string> records = stringData.Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records.Where(record =>
               record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

            string stringRecords = string.Join(Environment.NewLine, filteredRecords);

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

            List<Address> addresses = await this.csvHelperBroker
                .MapCsvToObjectAsync<Address>(stringRecords, hasHeaderRecord: false, fieldMappings);

            return addresses;
        }

        virtual internal async ValueTask<List<Address>> MapStreetDescriptorDataToAddressesAsync(
            string streetDescriptorCsvFile)
        {
            bool fileExists = await this.fileBroker.CheckIfFileExistsAsync(streetDescriptorCsvFile);

            if (!fileExists)
            {
                throw new InvalidFileAddressOrchestrationException(
                    message: $"The file {streetDescriptorCsvFile} could not be found.");
            }

            byte[] csvData = await fileBroker.ReadFileAsync(streetDescriptorCsvFile);
            string stringData = Encoding.UTF8.GetString(csvData);

            List<string> records = stringData.Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records.Where(record =>
                record.StartsWith("15,") || record.StartsWith("\"15\",")).ToList();

            string stringRecords = string.Join(Environment.NewLine, filteredRecords);

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { "USRN", 3 },
                { "StreetDescription", 4 },
                { "Locality", 5 },
                { "TownName", 6 },
            };

            List<StreetDescriptor> streetDescriptors = await this.csvHelperBroker
                .MapCsvToObjectAsync<StreetDescriptor>(stringRecords, hasHeaderRecord: false, fieldMappings);

            List<Address> addresses = [];

            foreach (StreetDescriptor streetDescriptor in streetDescriptors)
            {
                Address address = new Address
                {
                    USRN = streetDescriptor.USRN,
                    Thoroughfare = streetDescriptor.StreetDescription,
                    DependentLocality = streetDescriptor.Locality,
                    PostTown = streetDescriptor.TownName
                };

                addresses.Add(address);
            }

            return addresses;
        }

        public ValueTask SyncAddressesWithAssignAsync()
        {
            throw new NotImplementedException();
        }
    }
}
