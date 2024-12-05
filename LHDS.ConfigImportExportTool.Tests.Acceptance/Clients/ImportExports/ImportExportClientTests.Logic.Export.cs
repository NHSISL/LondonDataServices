// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;

namespace LHDS.ConfigImportExportTool.Tests.Acceptance.Clients.ImportExports
{
    public partial class ImportExportClientTests
    {
        [Fact]
        public async Task ShouldExportSchemaFileAsync()
        {
            //Given
            Supplier randomSupplier = CreateRandomSupplier();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification = 
                CreateRandomDataSetSpecification(randomDataSet.Id);

            List<DataSetSpecification> dataSetSpecifications = 
                new List<DataSetSpecification> { randomDataSetSpecification };

            dynamic dynamicSchemaItem = CreateRandomDynamicSchemaItem();

            SpecificationObject specificationObject = 
                CreateSpecificationObjectFromDynamic(dynamicSchemaItem, randomDataSetSpecification.Id);

            ObjectColumn objectColumn =
                CreateObjectColumnFromDynamic(dynamicSchemaItem, specificationObject.Id);

            randomDataSet.DataSetSpecifications = dataSetSpecifications;
            await this.storageBroker.InsertSupplierAsync(randomSupplier);
            await this.storageBroker.InsertDataSetAsync(randomDataSet);
            await this.storageBroker.InsertDataSetSpecificationAsync(randomDataSetSpecification);
            await this.storageBroker.InsertSpecificationObjectAsync(specificationObject);
            await this.storageBroker.InsertObjectColumnAsync(objectColumn);

            string assembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            char separator = Path.DirectorySeparatorChar;

            string outputFilePath = Path.Combine(
                assembly,
                $"Resources{separator}Clients{separator}ImportExport{separator}" +
                    "Test_Export_schema_file.csv");

            //When
            await this.importExportClient.Export(
                dataSetName: randomDataSet.DataSetName,
                version: randomDataSetSpecification.SupplierSpecificationVersion,
                filePath: outputFilePath);

            //Then
            byte[] csvData = await this.fileBroker.ReadFileAsync(outputFilePath);
            string csvString = Encoding.Default.GetString(csvData);

            List<CannonicalSchemaItem> cannonicalSchemaItems = 
                await this.csvHelperBroker.MapCsvToObjectAsync<CannonicalSchemaItem>(csvString, true);
        }
    }
}