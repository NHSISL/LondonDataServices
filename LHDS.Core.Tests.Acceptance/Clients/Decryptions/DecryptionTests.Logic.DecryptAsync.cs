// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldDecryptNewDocumentsAsync()
        {
            //Given
            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            Guid supplierId = Guid.NewGuid();
            byte[] documentData = Encoding.ASCII.GetBytes(GetRandomString());
            Stream inputStream = new MemoryStream(documentData);
            Stream encryptedStream = new MemoryStream();
            Stream decryptedStream = new MemoryStream();
            Supplier randomSupplier = CreateRandomSupplier(supplierId, dateTimeOffset);
            SubscriberCredential subscriberCredential = CreateRandomSubscriberCredential();

            SubscriberCredential generatedSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(subscriberCredential, regenerateKeys: true);

            await this.cryptographyProvider.EncryptAsync(inputStream, encryptedStream, generatedSubscriberCredential);
            string encryptedFileName = CreateRandomFileName(subscriberCredential.Id);
            string decryptedFileName = CreateRandomFileName(subscriberCredential.Id);
            await this.documentService.AddDocumentAsync(encryptedStream, encryptedFileName, blobContainers.EmisLanding);
            await this.supplierService.AddSupplierAsync(randomSupplier);
            DataSet randomDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDataSet.Id);
            await this.dataSetService.AddDataSetAsync(randomDataSet);
            await this.dataSetSpecificationService.AddDataSetSpecificationAsync(randomDataSetSpecification);
            
            List<SpecificationObject> randomSpecificationObject = 
                CreateRandomSpecificationObjects(randomDataSetSpecification.Id);

            foreach (var item in randomSpecificationObject)
            {
                List<ObjectColumn> objectColumns = CreateRandomObjectColumns(item.Id);
                item.ObjectColumns = objectColumns;
                await this.specificationObjectService.AddSpecificationObjectAsync(item);

                foreach (ObjectColumn column in item.ObjectColumns)
                {
                    await this.objectColumnService.AddObjectColumnAsync(column);
                }
            }

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync(),
                encryptedFileName,
                decryptedFileName,
                supplierId: supplierId,
                randomDataSetSpecification.Id);

            await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

            //When
            var actualString = await this.decryptionClient.DecryptAsync(encryptedFileName);

            //Then
            actualString.Should().BeEquivalentTo(ingestionTracking.DecryptedFileName);

            await this.documentService.RetrieveDocumentByFileNameAsync(
                output: decryptedStream,
                fileName: ingestionTracking.DecryptedFileName,
                container: blobContainers.Ingress);

            byte[] decryptedData = ReadAllBytesFromStream(decryptedStream);
            decryptedData.Should().BeEquivalentTo(documentData);

            IngestionTracking decryptedIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            IQueryable<IngestionTrackingAudit> allAudits = 
                await this.auditService.RetrieveAllIngestionTrackingAuditsAsync();

            var audits = allAudits
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id).ToList();

            foreach (var audit in audits)
            {
                await this.auditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

            await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId: subscriberCredential.Id);

            await this.documentService.RemoveDocumentByFileNameAsync(encryptedFileName, blobContainers.EmisLanding);

            await this.documentService.RemoveDocumentByFileNameAsync(
                ingestionTracking.DecryptedFileName, blobContainers.Ingress);

            IQueryable<SpecificationObject> retrievedSpecificationObjects =
                await this.specificationObjectService.RetrieveAllSpecificationObjectsAsync();

            List<SpecificationObject> specificationObjectList = retrievedSpecificationObjects
                .Include(specificationObject => specificationObject.ObjectColumns)
                .Where(specificationObject => specificationObject.DataSetSpecificationId == randomDataSetSpecification.Id).ToList();

            foreach (var specificationObject in specificationObjectList)
            {
                foreach (ObjectColumn objectColumn in specificationObject.ObjectColumns)
                {
                    await this.objectColumnService.RemoveObjectColumnByIdAsync(objectColumn.Id);
                }

                await this.specificationObjectService.RemoveSpecificationObjectByIdAsync(specificationObject.Id);
            }

            await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.dataSetService.RemoveDataSetByIdAsync(randomDataSet.Id);
            await this.supplierService.RemoveSupplierByIdAsync(supplierId: supplierId);
        }
    }
}
