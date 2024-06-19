// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
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
            Document randomDocument = CreateRandomDocument();
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(activeDataSetSpecification);

            //When
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(randomDocument, supplierId);

            //Then
            IngestionTracking ingestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            await this.supplierService.RemoveSupplierByIdAsync(supplierId);

            var decryptedFileName =
                $"/{landingConfiguration.DecryptedFolder}"
                + $"/{activeDataSet.DataSetName}"
                + $"/{activeDataSetSpecification?.Id}"
                + $"/{randomDocument.FileName}";

            await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                decryptedFileName,
                blobContainers.Versioner);
        }

        [Fact]
        public async Task ShouldNotProcessExistingDocumentAsync()
        {
            //Given
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Document randomDocument = CreateRandomDocument();
            string fileName = GetRandomFileName();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            Guid supplierId = Guid.NewGuid();
            Supplier landingSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(activeDataSetSpecification);

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTime, document, supplierId);

            randomIngestionTracking.FileName = randomDocument.FileName;
            randomIngestionTracking.DecryptedFileSha256Hash = randomDocument.SHA256Hash;
            await this.ingestionTrackingService.AddIngestionTrackingAsync(randomIngestionTracking);

            //When
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(randomDocument, supplierId);

            //Then
            IngestionTracking ingestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            await this.supplierService.RemoveSupplierByIdAsync(supplierId);
        }

        [Fact]
        public async Task ShouldProcessExistingDocumentAndUpdateHashAsync()
        {
            //Given
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Document randomDocument = CreateRandomDocument();
            string fileName = GetRandomFileName();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            Guid supplierId = Guid.NewGuid();
            Supplier landingSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);
            await this.supplierService.AddSupplierAsync(landingSupplier);
            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(activeDataSetSpecification);

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTime, document, supplierId);

            randomIngestionTracking.FileName = randomDocument.FileName;
            await this.ingestionTrackingService.AddIngestionTrackingAsync(randomIngestionTracking);

            //When
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(randomDocument, supplierId);

            //Then
            IngestionTracking ingestionTracking =
               await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            await this.supplierService.RemoveSupplierByIdAsync(landingSupplier.Id);

            await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                randomDocument.FileName,
                blobContainers.Versioner);
        }
    }
}
