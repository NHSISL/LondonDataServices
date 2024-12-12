// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.TppLandings
{
    public partial class TppLandingTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentAndAddHashAsync()
        {
            //Given
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid supplierId = Guid.NewGuid();
            Supplier landingSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            string randomContent = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomContent);
            Stream randomStream = new MemoryStream(randomBytes);
            Stream inputStream = randomStream;
            string inputFileName = GetRandomFileName();
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            SpecificationObject specificationObject = CreateRandomSpecificationObjects(activeDataSetSpecification);
            ObjectColumn objectColumn = CreateRandomObjectColumns(specificationObject);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(activeDataSetSpecification);
            await this.specificationObjectService.AddSpecificationObjectAsync(specificationObject);
            await this.objectColumnService.AddObjectColumnAsync(objectColumn);

            //When
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(
                input: inputStream,
                fileName: inputFileName,
                supplierId);

            //Then
            IngestionTracking ingestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            List<IngestionTrackingAudit> audits =
                this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id).ToList();

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.objectColumnService.RemoveObjectColumnByIdAsync(objectColumn.Id);
            await this.specificationObjectService.RemoveSpecificationObjectByIdAsync(specificationObject.Id);
            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            await this.supplierService.RemoveSupplierByIdAsync(supplierId);

            await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                ingestionTracking.DecryptedFileName,
                blobContainers.Ingress);
        }

        //[Fact(Skip = "Will fix later")]
        [Fact]
        public async Task ShouldNotProcessExistingDocumentAsync()
        {
            //Given
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            string randomFileName = GetRandomFileName();
            string fileName = randomFileName;
            string randomHash = GetRandomString();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream inputStream = new MemoryStream(documentData);
            Guid supplierId = Guid.NewGuid();
            Supplier landingSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            SpecificationObject specificationObject = CreateRandomSpecificationObjects(activeDataSetSpecification);
            ObjectColumn objectColumn = CreateRandomObjectColumns(specificationObject);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(activeDataSetSpecification);
            await this.specificationObjectService.AddSpecificationObjectAsync(specificationObject);
            await this.objectColumnService.AddObjectColumnAsync(objectColumn);

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTime, fileName, supplierId);

            randomIngestionTracking.FileName = randomFileName;
            randomIngestionTracking.DecryptedFileSha256Hash = randomHash;
            randomIngestionTracking.DataSetSpecificationId = activeDataSetSpecification.Id;
            await this.ingestionTrackingService.AddIngestionTrackingAsync(randomIngestionTracking);

            //When
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(
                input: inputStream,
                fileName,
                supplierId);

            //Then
            IngestionTracking ingestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            List<IngestionTrackingAudit> audits =
                this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id).ToList();

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.objectColumnService.RemoveObjectColumnByIdAsync(objectColumn.Id);
            await this.specificationObjectService.RemoveSpecificationObjectByIdAsync(specificationObject.Id);
            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            await this.supplierService.RemoveSupplierByIdAsync(supplierId);

            await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                ingestionTracking.DecryptedFileName,
                blobContainers.Ingress);
        }

        [Fact(Skip = "Will fix later")]
        public async Task ShouldProcessExistingDocumentAndUpdateHashAsync()
        {
            //Given
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            string randomFileName = GetRandomFileName();
            string fileName = randomFileName;
            string randomHash = GetRandomString();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream inputStream = new MemoryStream(documentData);
            Guid supplierId = Guid.NewGuid();
            Supplier landingSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            SpecificationObject specificationObject = CreateRandomSpecificationObjects(activeDataSetSpecification);
            ObjectColumn objectColumn = CreateRandomObjectColumns(specificationObject);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(activeDataSetSpecification);
            await this.specificationObjectService.AddSpecificationObjectAsync(specificationObject);
            await this.objectColumnService.AddObjectColumnAsync(objectColumn);

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTime, fileName, supplierId);

            randomIngestionTracking.FileName = randomFileName;
            randomIngestionTracking.DataSetSpecificationId = activeDataSetSpecification.Id;
            await this.ingestionTrackingService.AddIngestionTrackingAsync(randomIngestionTracking);

            //When
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(
                input: inputStream,
                fileName,
                supplierId);

            //Then
            IngestionTracking ingestionTracking =
               await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            List<IngestionTrackingAudit> audits =
                this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id).ToList();

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.objectColumnService.RemoveObjectColumnByIdAsync(objectColumn.Id);
            await this.specificationObjectService.RemoveSpecificationObjectByIdAsync(specificationObject.Id);
            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            await this.supplierService.RemoveSupplierByIdAsync(landingSupplier.Id);

            await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                ingestionTracking.DecryptedFileName,
                blobContainers.Ingress);
        }
    }
}
