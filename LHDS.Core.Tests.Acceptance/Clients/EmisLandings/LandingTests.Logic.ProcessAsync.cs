// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Tests.Acceptance.Clients.EmisLandings.Models;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.EmisLandings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            //Given
            CleanupDownloadFolder();
            DateTimeOffset randomDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
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

            List<DocumentSource> randomFiles = PrepareAndAddFile(
                subscriberAgreementId: inputSubscriberCredential.Id,
                dataSetSpecification: retrievedDataSetSpecification,
                createFiles: true,
                count: GetRandomNumber());

            List<string> expectedFiles = randomFiles.Select(file => file.DecryptedBlobPath).ToList();

            //When
            var actualStringList = await this.landingClient.ProcessAsync(supplierId);

            //Then
            expectedFiles.Should().BeEquivalentTo(actualStringList);

            foreach (var actualFile in actualStringList)
            {
                IQueryable<IngestionTracking> allIngestionTrackings = 
                    await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

                IngestionTracking ingestionTracking = allIngestionTrackings
                    .FirstOrDefault(ingestionTracking => ingestionTracking.DecryptedFileName == actualFile);

                ingestionTracking.Should().NotBeNull();

                await this.documentService.RemoveDocumentByFileNameAsync(
                    filename: ingestionTracking.EncryptedFileName,
                    container: blobContainers.EmisLanding);

                var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id).ToList();

                foreach (var audit in audits)
                {
                    await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            }

            await this.dataSetSpecificationProcessingService
                .RemoveDataSetSpecificationByIdAsync(activeDataSetSpecifications.Id);

            await this.dataSetService.RemoveDataSetByIdAsync(randomDataSet.Id);

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);

            CleanupDownloadFolder();
        }

        [Fact]
        public async Task ShouldNotProcessExistingDocumentsAsync()
        {
            //Given
            CleanupDownloadFolder();
            DateTimeOffset randomDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            Guid supplierId = Guid.NewGuid();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            DataSet randomDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(randomDataSet);
            Supplier randomSupplier = CreateRandomSupplier(supplierId, randomDateTime);
            SpecificationObject specificationObject = CreateRandomSpecificationObjects(activeDataSetSpecification);
            ObjectColumn objectColumn = CreateRandomObjectColumns(specificationObject);
            await this.supplierService.AddSupplierAsync(randomSupplier);
            await this.dataSetService.AddDataSetAsync(randomDataSet);
            await this.dataSetSpecificationProcessingService.AddDataSetSpecificationAsync(activeDataSetSpecification);
            await this.specificationObjectService.AddSpecificationObjectAsync(specificationObject);
            await this.objectColumnService.AddObjectColumnAsync(objectColumn);

            SubscriberCredential inputSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: randomSubscriberCredential,
                    regenerateKeys: true,
                    externalUse: false);

            DataSetSpecification retrievedDataSetSpecification =
                await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

            List<DocumentSource> documentSources = PrepareAndAddFile(
                subscriberAgreementId: inputSubscriberCredential.Id,
                dataSetSpecification: retrievedDataSetSpecification,
                createFiles: false,
                count: GetRandomNumber());

            List<IngestionTracking> ingestionTrackings = await CreateRandomIngestionTrackings(
                documentSources,
                supplierId: supplierId);

            //When
            var actualStringList = await this.landingClient.ProcessAsync(supplierId);

            //Then
            actualStringList.Should().HaveCount(0);

            foreach (var tracking in ingestionTrackings)
            {
                var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                    .Where(audit => audit.IngestionTrackingId == tracking.Id).ToList();

                foreach (var audit in audits)
                {
                    await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(tracking.Id);
            }

            await this.objectColumnService.RemoveObjectColumnByIdAsync(objectColumn.Id);
            await this.specificationObjectService.RemoveSpecificationObjectByIdAsync(specificationObject.Id);

            await this.dataSetSpecificationProcessingService
                .RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);

            await this.dataSetService.RemoveDataSetByIdAsync(randomDataSet.Id);

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: inputSubscriberCredential.Id);

            await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);

            CleanupDownloadFolder();
        }
    }
}
