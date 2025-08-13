// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Landings
{
    public partial class EmisLandingsApiTests
    {
        [Fact(Skip = "Hassan to fix")]
        public async Task ShouldReLandDocumentByFileNameForExistingIngestionTrackingAsync()
        {
            await ValueTask.CompletedTask;

            ////Given
            //SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            //SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            //await this.apiBroker.PostSubscriberCredentialAsync(inputSubscriberCredential);
            //string randomFileName = GetRandomFileName(inputSubscriberCredential.Id);
            //string randomFilePath = CreateRandomFilePath(inputSubscriberCredential.Id, randomFileName);
            //Supplier randomSupplier = await PostRandomSupplierAsync();
            //string encryptedFilePath = $"{encryptedFolder}/{randomFilePath}";
            //string decryptedFilePath = $"{decryptedFolder}/{randomFilePath}";
            //string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);
            //string testFilePath = Path.Combine(defaultFolderPath, randomFilePath.Replace("/", "\\"));
            //FileInfo fileInfo = new FileInfo(testFilePath);

            //if (!fileInfo.Directory.Exists)
            //{
            //    fileInfo.Directory.Create();
            //}

            //File.WriteAllText(testFilePath, GetRandomString());

            //Document document = new Document
            //{
            //    FileName = randomFilePath,
            //    DocumentData = Encoding.ASCII.GetBytes(GetRandomString()),
            //};

            //await this.apiBroker.documentService.AddDocumentAsync(document, "emislanding");

            //IngestionTracking randomIngestionTracking =
            //    await PostRandomIngestionTrackingAsync(
            //        randomSupplier.Id,
            //        randomFilePath,
            //        encryptedFilePath,
            //        decryptedFilePath);

            //IngestionTracking inputIngestionTracking = randomIngestionTracking;
            //IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            ////When
            //string actualDecryptedFileName =
            //    await this.apiBroker.ReLandDocumentByFileNameAsync(randomFilePath);

            ////Then
            //actualDecryptedFileName.Should().BeEquivalentTo(expectedIngestionTracking.DecryptedFileName);
            //await CleanupTask(expectedIngestionTracking.Id);
            //await this.apiBroker.documentService.RemoveDocumentByFileNameAsync(randomFilePath, "emislanding");
            //File.Delete(testFilePath);
        }

        [Fact(Skip = "Hassan to fix")]
        public async Task ShouldLandNewDocumentByFileNameForExistingIngestionTrackingAsync()
        {
            await ValueTask.CompletedTask;

            ////Given
            //SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            //SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            //await this.apiBroker.PostSubscriberCredentialAsync(inputSubscriberCredential);
            //string randomFileName = GetRandomFileName(inputSubscriberCredential.Id);
            //string randomFilePath = CreateRandomFilePath(inputSubscriberCredential.Id, randomFileName);
            //Guid emisSupplierId = Guid.Parse("67680f17-9d0c-4474-8b35-56ca8f9df1f6");
            //DataSet randomDataSet = await PostRandomActiveDataSetAsync(emisSupplierId);

            //DataSetSpecification randomDataSetSpecification =
            //    await PostRandomActiveDataSetSpecificationAsync(randomDataSet.Id);

            //string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);
            //string testFilePath = Path.Combine(defaultFolderPath, randomFilePath.Replace("/", "\\"));
            //FileInfo fileInfo = new FileInfo(testFilePath);

            //if (!fileInfo.Directory.Exists)
            //{
            //    fileInfo.Directory.Create();
            //}

            //File.WriteAllText(testFilePath, GetRandomString());

            //Document document = new Document
            //{
            //    FileName = randomFilePath,
            //    DocumentData = Encoding.ASCII.GetBytes(GetRandomString()),
            //};

            //await this.apiBroker.documentService.AddDocumentAsync(document, "emislanding");

            ////When
            //string actualDecryptedFileName =
            //    await this.apiBroker.ReLandDocumentByFileNameAsync(randomFilePath);

            ////Then
            //await CleanupTask(randomFilePath);
            //await this.apiBroker.documentService.RemoveDocumentByFileNameAsync(randomFilePath, "emislanding");
            //await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            //await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            //File.Delete(testFilePath);
        }

        [Fact]
        public async Task ShouldRetrieveListOfDocumentsToProcessAsync()
        {
            //given 
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            await this.apiBroker.PostSubscriberCredentialAndGenerateKeysAsync(inputSubscriberCredential);
            string randomFileName = GetRandomFileName(inputSubscriberCredential.Id);
            string randomFilePath = CreateRandomFilePath(inputSubscriberCredential.Id, randomFileName);
            Guid emisSupplierId = Guid.Parse("67680f17-9d0c-4474-8b35-56ca8f9df1f6");
            Supplier randomSupplier = await PostRandomSupplierAsync(supplierId: emisSupplierId);
            DataSet randomDataSet = await PostRandomActiveDataSetAsync(emisSupplierId);
            Stream randomStream = new MemoryStream();

            DataSetSpecification randomDataSetSpecification =
                await PostRandomActiveDataSetSpecificationAsync(randomDataSet.Id);

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);
            string testFilePath = Path.Combine(defaultFolderPath, randomFilePath.Replace("/", "\\"));
            FileInfo fileInfo = new FileInfo(testFilePath);

            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            File.WriteAllText(testFilePath, GetRandomString());

            // when
            List<string> actualDocuments =
                await this.apiBroker.RetrieveListOfDocumentsToProcessAsync(inputSubscriberCredential.Id);

            // then
            actualDocuments.Count.Should().BeGreaterThan(0);
            await CleanupTask(randomFilePath);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(inputSubscriberCredential.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            File.Delete(testFilePath);
        }

        [Fact(Skip = "Will fix in another PR.")]
        public async Task ShouldProcessDocumentsWithNewIngestionTrackingAsync()
        {
            //given 
            int randomFilesNumber = GetRandomNumber();
            List<Guid> subscriberCredentialIds = new List<Guid>();
            List<string> testFilePaths = new List<string>();

            for (int i = 0; i < randomFilesNumber; i++)
            {
                SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
                SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
                await this.apiBroker.PostSubscriberCredentialAndGenerateKeysAsync(inputSubscriberCredential);
                subscriberCredentialIds.Add(inputSubscriberCredential.Id);
                string randomFileName = GetRandomFileName(inputSubscriberCredential.Id);
                string randomFilePath = CreateRandomFilePath(inputSubscriberCredential.Id, randomFileName);
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);
                string testFilePath = Path.Combine(defaultFolderPath, randomFilePath.Replace("/", "\\"));
                FileInfo fileInfo = new FileInfo(testFilePath);

                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                File.WriteAllText(testFilePath, GetRandomString());
                testFilePaths.Add(testFilePath);
            }

            Guid emisSupplierId = Guid.Parse("67680f17-9d0c-4474-8b35-56ca8f9df1f6");
            DataSet randomDataSet = await PostRandomActiveDataSetAsync(emisSupplierId);

            DataSetSpecification randomDataSetSpecification =
                await PostRandomActiveDataSetSpecificationAsync(randomDataSet.Id);

            // when
            List<string> actualDocuments =
                await this.apiBroker.DownloadDocumentsAsync(emisSupplierId);

            // then
            actualDocuments.Count.Should().BeGreaterThanOrEqualTo(randomFilesNumber);

            foreach (Guid id in subscriberCredentialIds)
            {
                await this.apiBroker.DeleteSubscriberCredentialByIdAsync(id);
            }
            ;

            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);

            foreach (string testFilePath in testFilePaths)
            {
                File.Delete(testFilePath);

            }
        }

        [Fact]
        public async Task ShouldRedecryptDocumentByIngestionTrackingAsync()
        {
            //given 
            Supplier randomSupplier = await PostRandomSupplierAsync();
            string fileName = GetRandomString(10);
            string encryptedFilePath = "encrypted";
            string decryptedFilePath = "decrypted";

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(
                    supplierId: randomSupplier.Id,
                    fileName,
                    encryptedFilePath,
                    decryptedFilePath);

            randomIngestionTracking.Decrypted = true;
            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            // when
            await this.apiBroker.RedecryptDocumentByIngestionTrackingIdAsync(randomIngestionTracking.Id);

            // then
            IngestionTracking redecryptedIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            redecryptedIngestionTracking.Decrypted.Should().BeFalse();
            await CleanupTask(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        private async ValueTask CleanupTask(string fileName)
        {
            IngestionTracking maybeIngestionTracking =
                await this.apiBroker.FindIngestionTrackingByFileNameAsync(fileName);

            if (maybeIngestionTracking == null)
            {
                return;
            }

            var ingestionTrackingAudits = await this.apiBroker
                .FindIngestionTrackingAuditByIngestionTrackingIdAsync(maybeIngestionTracking.Id);

            foreach (var ingestionTrackingAudit in ingestionTrackingAudits)
            {
                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(ingestionTrackingAudit.Id);
            }

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(maybeIngestionTracking.Id);
        }

        private async ValueTask CleanupTask(Guid ingestionTrackingId)
        {
            var maybeIngestionTracking = await this.apiBroker.GetIngestionTrackingByIdAsync(ingestionTrackingId);

            if (maybeIngestionTracking == null)
            {
                return;
            }

            var ingestionTrackingAudits = await this.apiBroker
                .FindIngestionTrackingAuditByIngestionTrackingIdAsync(maybeIngestionTracking.Id);

            foreach (var ingestionTrackingAudit in ingestionTrackingAudits)
            {
                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(ingestionTrackingAudit.Id);
            }

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(maybeIngestionTracking.Id);
        }
    }
}