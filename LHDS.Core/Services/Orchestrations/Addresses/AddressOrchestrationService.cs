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
                            string nestedExtractPath =
                                Path.Combine(extractPath, Path.GetFileNameWithoutExtension(entry.FullName));

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

        virtual internal async ValueTask ReadCsvDataAndBulkAddAddressesAsync(string tempFolder)
        {
            List<string> csvFiles = await fileBroker.GetListOfFilesAsync(tempFolder, "*.csv");
            string dpaCsvFile = csvFiles.Where(csv => csv.Contains("ID28")).FirstOrDefault();
            string lpiCsvFile = csvFiles.Where(csv => csv.Contains("ID24")).FirstOrDefault();
            string blpuCsvFile = csvFiles.Where(csv => csv.Contains("ID21")).FirstOrDefault();
            string streetDescriptorCsvFile = csvFiles.Where(csv => csv.Contains("ID15")).FirstOrDefault();
            await ProcessDPAAddressesAsync(dpaCsvFile);
            await ProcessLPIAddressesAsync(lpiCsvFile);
            await ProcessBLPUAddressesAsync(blpuCsvFile);
            await ProcessStreetDescriptorDataAsync(streetDescriptorCsvFile);
        }

        virtual internal async ValueTask<List<T>> LoadAndMapCsvAsync<T>(
            string filePath,
            Dictionary<string, int> fieldMappings,
            Func<string, bool> recordFilterPredicate)
        {
            bool fileExists = await this.fileBroker.CheckIfFileExistsAsync(filePath);

            if (!fileExists)
            {
                throw new InvalidFileAddressOrchestrationException(
                    message: $"The file {filePath} could not be found.");
            }

            byte[] csvData = await fileBroker.ReadFileAsync(filePath);
            string stringData = Encoding.UTF8.GetString(csvData);

            List<string> records = stringData.Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<string> filteredRecords = records
                .Where(recordFilterPredicate)
                .ToList();

            string filteredCsv = string.Join(Environment.NewLine, filteredRecords);

            List<T> mappedObjects = await this.csvHelperBroker
                .MapCsvToObjectAsync<T>(filteredCsv, hasHeaderRecord: false, fieldMappings);

            return mappedObjects;
        }

        virtual internal async ValueTask<List<Address>> MapLPIDataToAddressesAsync(string lpiCsvFile)
        {
            Func<string, bool> recordFilter = record =>
                record.StartsWith("24,") || record.StartsWith("\"24\",");

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { "UPRN", 3 },
                { "LogicalStatus", 6 },
                { "StartDate", 7 },
                { "EndDate", 8 },
                { "SAOStartNumber", 11 },
                { "SAOStartSuffix", 12 },
                { "SAOEndNumber", 13 },
                { "SAOEndSuffix", 14 },
                { "SAOText", 15 },
                { "PAOStartNumber", 16 },
                { "PAOStartSuffix", 17 },
                { "PAOEndNumber", 18 },
                { "PAOEndSuffix", 19 },
                { "PAOText", 20 },
                { "USRN", 21 },
            };

            List<LPIAddress> lpiAddresses = await LoadAndMapCsvAsync<LPIAddress>(
                lpiCsvFile,
                fieldMappings,
                recordFilter);

            var lpiAddressesWithoutDuplicates = lpiAddresses
                .OrderBy(address => address.LogicalStatus)
                .ThenBy(address => address.EndDate == null)
                .ThenByDescending(address => address.EndDate)
                .GroupBy(address => address.UPRN)
                .Select(group => group.FirstOrDefault());

            List<Address> addresses = [];

            foreach (LPIAddress lpiAddress in lpiAddressesWithoutDuplicates)
            {
                Address address = MapLPIAddressToAddress(lpiAddress);
                addresses.Add(address);
            }

            return addresses;
        }

        virtual internal Address MapLPIAddressToAddress(LPIAddress lpiAddress)
        {
            string subBuildingName = "";
            string buildingName = "";
            string buildingNumber = "";

            if (!string.IsNullOrWhiteSpace(lpiAddress.SAOText))
            {
                subBuildingName = lpiAddress.SAOText;
            }
            else if (!string.IsNullOrWhiteSpace(lpiAddress.SAOEndNumber + lpiAddress.SAOEndSuffix))
            {
                subBuildingName = string.IsNullOrWhiteSpace(lpiAddress.SAOEndSuffix + lpiAddress.SAOStartSuffix)
                    ? $"{lpiAddress.SAOStartNumber}-{lpiAddress.SAOEndNumber}"
                    : $"{lpiAddress.SAOStartNumber}{lpiAddress.SAOStartSuffix}-" +
                        $"{lpiAddress.SAOEndNumber}{lpiAddress.SAOEndSuffix}";
            }
            else if (!string.IsNullOrWhiteSpace(lpiAddress.SAOStartNumber + lpiAddress.SAOStartSuffix))
            {
                subBuildingName = string.IsNullOrWhiteSpace(lpiAddress.SAOStartSuffix)
                    ? lpiAddress.SAOStartNumber
                    : $"{lpiAddress.SAOStartNumber}{lpiAddress.SAOStartSuffix}";
            }

            if (!string.IsNullOrWhiteSpace(lpiAddress.PAOText))
            {
                buildingName = lpiAddress.PAOText;
            }
            else if (string.IsNullOrWhiteSpace(
                lpiAddress.PAOStartSuffix + lpiAddress.PAOEndNumber + lpiAddress.PAOEndSuffix))
            {
                buildingNumber = lpiAddress.PAOStartNumber;
            }
            else if (!string.IsNullOrWhiteSpace(lpiAddress.PAOEndNumber + lpiAddress.PAOEndSuffix))
            {
                buildingName = string.IsNullOrWhiteSpace(lpiAddress.PAOEndSuffix + lpiAddress.PAOStartSuffix)
                    ? $"{lpiAddress.PAOStartNumber}-{lpiAddress.PAOEndNumber}"
                    : $"{lpiAddress.PAOStartNumber}{lpiAddress.PAOStartSuffix}-" +
                        $"{lpiAddress.PAOEndNumber}{lpiAddress.PAOEndSuffix}";
            }
            else if (!string.IsNullOrWhiteSpace(lpiAddress.PAOStartNumber + lpiAddress.PAOStartSuffix))
            {
                buildingName = string.IsNullOrWhiteSpace(lpiAddress.PAOStartSuffix)
                    ? lpiAddress.PAOStartNumber
                    : $"{lpiAddress.PAOStartNumber}{lpiAddress.PAOStartSuffix}";
            }

            Address address = new Address
            {
                UPRN = lpiAddress.UPRN,
                USRN = lpiAddress.USRN,
                SubBuildingName = subBuildingName,
                BuildingName = buildingName,
                BuildingNumber = buildingNumber
            };

            return address;
        }

        virtual internal async ValueTask ProcessDPAAddressesAsync(string dpaCsvFile)
        {
            List<Address> dpaAddresses = await MapDPADataToAddressesAsync(dpaCsvFile);
            Dictionary<string, Address> dpaAddressesDict = dpaAddresses.ToDictionary(a => a.UPRN, a => a);
            IQueryable<Address> addresses = await addressProcessingService.RetrieveAllAddressesAsync();
            HashSet<string> dpaFileUprns = dpaAddresses.Select(a => a.UPRN).ToHashSet();

            List<Address> existingDpaAddresses = addresses.Where(address =>
                dpaFileUprns.Contains(address.UPRN)).ToList();

            HashSet<string> existingDpaUprns = existingDpaAddresses.Select(a => a.UPRN).ToHashSet();

            List<Address> newDpaAddresses = dpaAddresses.Where(dpaAddress =>
                !existingDpaUprns.Contains(dpaAddress.UPRN)).ToList();

            foreach (Address address in existingDpaAddresses)
            {
                if (address.UPRN != null && dpaAddressesDict.TryGetValue(address.UPRN, out Address dpaAddress))
                {
                    address.OrganisationName = dpaAddress.OrganisationName;
                    address.DepartmentName = dpaAddress.DepartmentName;
                    address.SubBuildingName = dpaAddress.SubBuildingName;
                    address.BuildingName = dpaAddress.BuildingName;
                    address.BuildingNumber = dpaAddress.BuildingNumber;
                    address.DependentThoroughfare = dpaAddress.DependentThoroughfare;
                    address.Thoroughfare = dpaAddress.Thoroughfare;
                    address.DoubleDependentLocality = dpaAddress.DoubleDependentLocality;
                    address.DependentLocality = dpaAddress.DependentLocality;
                    address.PostTown = dpaAddress.PostTown;
                    address.PostCode = dpaAddress.PostCode;
                }
            }

            await addressProcessingService.BulkAddAddressesAsync(newDpaAddresses, dpaCsvFile);
            await addressProcessingService.BulkModifyAddressesAsync(existingDpaAddresses, dpaCsvFile);
        }

        virtual internal async ValueTask<List<Address>> MapDPADataToAddressesAsync(string dpaCsvFile)
        {
            Func<string, bool> recordFilter = record =>
                record.StartsWith("28,") || record.StartsWith("\"28\",");

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

            List<Address> addresses = await LoadAndMapCsvAsync<Address>(
                dpaCsvFile,
                fieldMappings,
                recordFilter);

            return addresses;
        }

        virtual internal async ValueTask<List<Address>> MapBLPUDataToAddressesAsync(string blpuCsvFile)
        {
            Func<string, bool> recordFilter = record =>
                record.StartsWith("21,") || record.StartsWith("\"21\",");

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { "UPRN", 3 },
                { "LogicalStatus", 4 },
                { "StartDate", 15 },
                { "EndDate", 16 },
                { "PostCode", 20 },
            };

            List<BLPUAddress> blpuAddresses = await LoadAndMapCsvAsync<BLPUAddress>(
                blpuCsvFile,
                fieldMappings,
                recordFilter);

            var blpuAddressesWithoutDuplicates = blpuAddresses
                .OrderBy(address => address.LogicalStatus)
                .ThenBy(address => address.EndDate == null)
                .ThenByDescending(address => address.EndDate)
                .GroupBy(address => address.UPRN)
                .Select(group => group.FirstOrDefault());

            List<Address> addresses = [];

            foreach (BLPUAddress blpuAddress in blpuAddressesWithoutDuplicates)
            {
                Address address = new Address
                {
                    UPRN = blpuAddress.UPRN,
                    PostCode = blpuAddress.PostCode,
                };

                addresses.Add(address);
            }

            return addresses;
        }

        virtual internal async ValueTask ProcessLPIAddressesAsync(string lpiCsvFile)
        {
            List<Address> lpiAddresses = await MapLPIDataToAddressesAsync(lpiCsvFile);
            Dictionary<string, Address> lpiAddressesDict = lpiAddresses.ToDictionary(a => a.UPRN, a => a);
            IQueryable<Address> addresses = await addressProcessingService.RetrieveAllAddressesAsync();
            HashSet<string> lpiFileUprns = lpiAddresses.Select(a => a.UPRN).ToHashSet();

            List<Address> existingLpiAddresses = addresses.Where(address =>
                lpiFileUprns.Contains(address.UPRN)).ToList();

            HashSet<string> existingLpiUprns = existingLpiAddresses.Select(a => a.UPRN).ToHashSet();

            List<Address> newLpiAddresses = lpiAddresses.Where(lpiAddress =>
                !existingLpiUprns.Contains(lpiAddress.UPRN)).ToList();

            foreach (Address existingAddress in existingLpiAddresses)
            {
                if (existingAddress.UPRN != null && lpiAddressesDict.TryGetValue(existingAddress.UPRN, out Address lpiAddress))
                {
                    existingAddress.USRN = lpiAddress.USRN;
                }
            }

            await addressProcessingService.BulkAddAddressesAsync(newLpiAddresses, lpiCsvFile);
            await addressProcessingService.BulkModifyAddressesAsync(existingLpiAddresses, lpiCsvFile);
        }

        virtual internal async ValueTask ProcessBLPUAddressesAsync(string blpuCsvFile)
        {
            List<Address> blpuAddresses = await MapBLPUDataToAddressesAsync(blpuCsvFile);
            Dictionary<string, Address> blpuAddressesDict = blpuAddresses.ToDictionary(a => a.UPRN, a => a);
            IQueryable<Address> addresses = await addressProcessingService.RetrieveAllAddressesAsync();
            HashSet<string> blpuFileUprns = blpuAddresses.Select(a => a.UPRN).ToHashSet();

            List<Address> existingBlpuAddresses = addresses.Where(address =>
                blpuFileUprns.Contains(address.UPRN)).ToList();

            HashSet<string> existingBlpuUprns = existingBlpuAddresses.Select(a => a.UPRN).ToHashSet();

            List<Address> newBlpuAddresses = blpuAddresses.Where(blpuAddress =>
                !existingBlpuUprns.Contains(blpuAddress.UPRN)).ToList();

            List<Address> updatedBlpuAddress = [];

            foreach (Address existingAddress in existingBlpuAddresses)
            {
                if (existingAddress.UPRN != null
                    && blpuAddressesDict.TryGetValue(existingAddress.UPRN, out Address blpuAddress)
                    && string.IsNullOrWhiteSpace(existingAddress.PostCode))
                {
                    existingAddress.PostCode = blpuAddress.PostCode;
                    updatedBlpuAddress.Add(existingAddress);
                }
            }

            await addressProcessingService.BulkAddAddressesAsync(newBlpuAddresses, blpuCsvFile);
            await addressProcessingService.BulkModifyAddressesAsync(updatedBlpuAddress, blpuCsvFile);
        }

        virtual internal async ValueTask ProcessStreetDescriptorDataAsync(string streetDescriptorCsvFile) =>
            throw new NotImplementedException();

        public ValueTask SyncAddressesWithAssignAsync()
        {
            throw new NotImplementedException();
        }
    }
}
