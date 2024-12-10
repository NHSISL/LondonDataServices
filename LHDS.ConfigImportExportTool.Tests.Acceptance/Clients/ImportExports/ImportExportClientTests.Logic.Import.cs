// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Acceptance.Clients.ImportExports
{
    public partial class ImportExportClientTests
    {
        [Fact(Skip = "Hassan can you fix please")]
        public async Task ShouldImportSchemaFileAsync()
        {
            //Given
            Supplier randomSupplier = CreateRandomSupplier();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplier.Id);
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDataSet.Id);

            List<DataSetSpecification> dataSetSpecifications =
                new List<DataSetSpecification> { randomDataSetSpecification };

            randomDataSet.DataSetSpecifications = dataSetSpecifications;
            await this.storageBroker.InsertSupplierAsync(randomSupplier);
            await this.storageBroker.InsertDataSetAsync(randomDataSet);
            await this.storageBroker.InsertDataSetSpecificationAsync(randomDataSetSpecification);
            string assembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            char separator = Path.DirectorySeparatorChar;

            string inputFilePath = Path.Combine(
                assembly,
                $"Resources{separator}Clients{separator}ImportExport{separator}" +
                    "Test_schema_file.csv");

            //When
            await this.importExportClient.Import(
                dataSetName: randomDataSet.DataSetName,
                version: randomDataSetSpecification.SupplierSpecificationVersion,
                filePath: inputFilePath);

            //Then
            IQueryable<SpecificationObject> specificationObjectsQuery =
                await this.storageBroker.SelectAllSpecificationObjectsAsync();

            List<SpecificationObject> specificationObjects = specificationObjectsQuery
                .Include(specificationObject => specificationObject.ObjectColumns)
                .Where(so => so.DataSetSpecificationId == randomDataSetSpecification.Id).ToList();

            foreach (SpecificationObject specificationObject in specificationObjects)
            {
                foreach (ObjectColumn objectColumn in specificationObject.ObjectColumns)
                {
                    await this.storageBroker.DeleteObjectColumnAsync(objectColumn);
                }

                await this.storageBroker.DeleteSpecificationObjectAsync(specificationObject);
            }

            await this.storageBroker.DeleteDataSetSpecificationAsync(randomDataSetSpecification);
            await this.storageBroker.DeleteDataSetAsync(randomDataSet);
        }
    }
}
