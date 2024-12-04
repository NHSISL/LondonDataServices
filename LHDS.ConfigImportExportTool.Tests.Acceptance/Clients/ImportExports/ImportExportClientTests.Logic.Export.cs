// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Reflection;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;

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

            List<SpecificationObject> specificationObjects = 
                CreateRandomSpecificationObjects(randomDataSetSpecification.Id);

            randomDataSet.DataSetSpecifications = dataSetSpecifications;
            await this.storageBroker.InsertSupplierAsync(randomSupplier);
            await this.storageBroker.InsertDataSetAsync(randomDataSet);
            await this.storageBroker.InsertDataSetSpecificationAsync(randomDataSetSpecification);

            foreach(var specificationObject in specificationObjects)
            {
                List<ObjectColumn> createdObjectColumns = 
                    CreateRandomObjectColumns(specificationObject.Id);

                specificationObject.ObjectColumns = createdObjectColumns;
                await this.storageBroker.InsertSpecificationObjectAsync(specificationObject);

                foreach(var createdObjectColumn in createdObjectColumns)
                {
                    await this.storageBroker.InsertObjectColumnAsync(createdObjectColumn);
                }
            }

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
        }
    }
}