// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
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
            Guid supplierId = landingConfiguration.LandingSupplierId;
            Supplier landingSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            Document randomDocument = CreateRandomDocument();
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);

            await this.supplierService.AddSupplierAsync(landingSupplier);
            await this.dataSetService.AddDataSetAsync(activeDataSet);
            await this.dataSetSpecificationProcessingService.AddDataSetSpecificationAsync(activeDataSetSpecification);

            //When
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(randomDocument);

            //Then
            IngestionTracking ingestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            Assert.Equal(randomDocument.FileName, ingestionTracking.FileName);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            await this.dataSetSpecificationProcessingService
                .RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);

            await this.dataSetService.RemoveDataSetByIdAsync(activeDataSet.Id);
            await this.supplierService.RemoveSupplierByIdAsync(landingSupplier.Id);
            await this.documentProcessingService.RemoveDocumentByFileNameAsync(randomDocument.FileName, "tpplanding");
        }

        [Fact]
        public async Task ShouldProcessExistingDocumentAndUpdateHashAsync()
        {
            //Given
            // Get Random Document
            //Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();


            // Create Documents
            // Create a few records in IngestionTracking

            // Populate 1 of those records with Same SHA

            // Retrieve All

            //When
            //Guid actualGuid = await this.tppLandingClient.ProcessAsync(randomDocument)

            //Then

            //use guid to lookup ingestion
            //asset filename match and hash
            // use guid delete all audits
            // use guid delete ingestion

            // read bytes and match
            // delete document

        }
    }
}
