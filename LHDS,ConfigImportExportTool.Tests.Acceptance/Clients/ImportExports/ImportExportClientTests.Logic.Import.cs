// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Reflection;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;

namespace LHDS.ConfigImportExportTool.Tests.Acceptance.Clients.ImportExports
{
    public partial class ImportExportClientTests
    {
        [Fact]
        public async Task ShouldImportSchemaFileAsync()
        {
            //Given
            Supplier randomSupplier = CreateRandomSupplier();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplier.Id);
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDataSet.Id);
            await this.storageBroker.InsertSupplierAsync(randomSupplier);
            await this.storageBroker.InsertDataSetAsync(randomDataSet);
            await this.storageBroker.InsertDataSetSpecificationAsync(randomDataSetSpecification);
            string assembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            char separator = Path.DirectorySeparatorChar;

            string inputFilePath = Path.Combine(
                assembly,
                $"Resource{separator}Clients{separator}ImportExports{separator}" +
                    "Test_schema_file.csv");

            //When
            await this.importExportClient.Import(
                dataSetName: randomDataSet.DataSetName,
                version: randomDataSetSpecification.SupplierSpecificationVersion,
                filePath: inputFilePath);
        }
    }
}