// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldProcessDecryptedItemsForBatchCompleteAsync()
        {
            //Given
            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            Guid supplierId = Guid.NewGuid();
            Guid subscriberCredentialId = Guid.NewGuid();
            Supplier randomSupplier = CreateRandomSupplier(supplierId, dateTimeOffset);
            SubscriberAgreement subscriberAgreement = CreateRandomSubscriberAgreement(supplierId);
            await this.subscriberAgreementService.AddSubscriberAgreementAsync(subscriberAgreement);
            await this.supplierService.AddSupplierAsync(randomSupplier);
            DataSet dataSet = CreateRandomDataSet(supplierId);
            await this.dataSetService.AddDataSetAsync(dataSet);
            DataSetSpecification dataSetSpecification = CreateRandomDataSetSpecification(dataSet.Id);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(dataSetSpecification);
            SpecificationObject specificationObject = CreateRandomSpecificationObject(dataSetSpecification.Id);
            await this.specificationObjectService.AddSpecificationObjectAsync(specificationObject);
            string encryptedFileName = CreateRandomFileName(subscriberCredentialId);
            string decryptedFileName = CreateRandomFileName(subscriberCredentialId);

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync(),
                encryptedFileName,
                decryptedFileName,
                supplierId: supplierId,
                Guid.NewGuid(),
                blobContainers.Ingress);

            ingestionTracking.SubscriberAgreementId = subscriberAgreement.Id;
            ingestionTracking.DataSetSpecificationId = dataSetSpecification.Id;
            ingestionTracking.ObjectName = specificationObject.SupplierObjectName;
            ingestionTracking.IsBatchComplete = false;
            ingestionTracking.IsDownloaded = true;
            ingestionTracking.Decrypted = true;
            ingestionTracking.IsProcessing = false;
            ingestionTracking.RetryCount = 0;
            ingestionTracking.LastAttempt = dateTimeOffset.AddMinutes(-15);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            //When
            await this.decryptionClient.ProcessDecryptedItemsForBatchCompleteAsync();

            //Then
            IngestionTracking retrievedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            retrievedIngestionTracking.IsBatchComplete.Should().BeTrue();

            IngestionTracking decryptedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            decryptedIngestionTracking.IsBatchComplete.Should().BeTrue();

            IQueryable<IngestionTrackingAudit> allAudits =
                await this.auditService.RetrieveAllIngestionTrackingAuditsAsync();

            var audits = allAudits
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id).ToList();

            foreach (var audit in audits)
            {
                await this.auditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            await this.specificationObjectService.RemoveSpecificationObjectByIdAsync(specificationObject.Id);
            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(dataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(dataSet.Id);
            await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);
            await this.subscriberAgreementService.RemoveSubscriberAgreementByIdAsync(subscriberAgreement.Id);
        }
    }
}
