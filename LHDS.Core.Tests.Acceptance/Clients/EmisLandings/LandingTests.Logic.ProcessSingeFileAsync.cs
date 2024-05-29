// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Tests.Acceptance.Clients.EmisLandings.Models;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.EmisLandings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentAsync()
        {
            // Given
            CleanupDownloadFolder();
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid supplierId = Guid.NewGuid();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            DataSet randomDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecifications = CreateRandomDataSetSpecification(randomDataSet);
            Supplier randomSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            await this.supplierService.AddSupplierAsync(randomSupplier);
            await this.dataSetService.AddDataSetAsync(randomDataSet);
            await this.dataSetSpecificationProcessingService.AddDataSetSpecificationAsync(activeDataSetSpecifications);


            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: randomSubscriberCredential,
                    regenerateKeys: true,
                    externalUse: false);

            DataSetSpecification retrievedDataSetSpecification =
                await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

            DocumentSource randomFile = PrepareAndAddFile(
                subscriberAgreementId: inputSubscriberCredential.Id,
                dataSetSpecification: retrievedDataSetSpecification,
                createFiles: true,
                count: 1).First();

            string inputFileName = randomFile.FtpPath;
            string expectedString = randomFile.DecryptedBlobPath;

            // When
            var actualString = await this.landingClient.ProcessAsync(fileName: inputFileName, supplierId);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);

            IngestionTracking retrievedInestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(inputFileName);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == retrievedInestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.dataSetSpecificationProcessingService
               .RemoveDataSetSpecificationByIdAsync(activeDataSetSpecifications.Id);

            await this.dataSetService.RemoveDataSetByIdAsync(randomDataSet.Id);

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(retrievedInestionTracking.Id);
            CleanupDownloadFolder();

            await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);
        }

        [Fact]
        public async Task ShouldProcessExistingDocumentAsync()
        {
            // Given
            CleanupDownloadFolder();
            DateTimeOffset randomDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid supplierId = Guid.NewGuid();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            DataSet randomDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecifications = CreateRandomDataSetSpecification(randomDataSet);
            Supplier randomSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            await this.supplierService.AddSupplierAsync(randomSupplier);
            await this.dataSetService.AddDataSetAsync(randomDataSet);
            await this.dataSetSpecificationProcessingService.AddDataSetSpecificationAsync(activeDataSetSpecifications);

            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: randomSubscriberCredential,
                    regenerateKeys: true,
                    externalUse: false);

            DataSetSpecification retrievedDataSetSpecification =
                await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

            DocumentSource randomFile = PrepareAndAddFile(
                subscriberAgreementId: inputSubscriberCredential.Id,
                dataSetSpecification: retrievedDataSetSpecification,
                createFiles: true,
                count: 1).First();

            List<IngestionTracking> ingestionTrackings = await CreateRandomIngestionTrackings(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                documentSources: new List<DocumentSource> { randomFile },
                supplierId: landingConfiguration.LandingSupplierId);

            string inputFileName = randomFile.FtpPath;
            string expectedString = randomFile.DecryptedBlobPath;

            // When
            var actualString = await this.landingClient.ProcessAsync(fileName: inputFileName, supplierId);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);

            IngestionTracking retrievedInestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(inputFileName);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == retrievedInestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.dataSetSpecificationProcessingService
               .RemoveDataSetSpecificationByIdAsync(activeDataSetSpecifications.Id);

            await this.dataSetService.RemoveDataSetByIdAsync(randomDataSet.Id);

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(retrievedInestionTracking.Id);
            CleanupDownloadFolder();

            await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);
        }
    }
}
