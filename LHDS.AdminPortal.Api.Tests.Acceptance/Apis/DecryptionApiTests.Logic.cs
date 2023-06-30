// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Decryptions
{
    public partial class DecryptionsApiTests
    {
        [Fact]
        public async Task ShouldDecryptFileAsync()
        {
            [Fact]
            public async Task ShouldDecryptFileAsync()
            {
                //Given
                Supplier randomSupplier = await PostRandomSupplierAsync();
                IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
                await DeleteAuditRecordsAsync(randomIngestionTracking);
                string randomFileName = GetRandomString();

                Document document = new Document
                {
                    DocumentData = encryptedData,
                    FileName = randomFileName
                };

                IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                    dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                    document,
                    supplierId: this.landingConfiguration.LandingSupplierId);

                await this.apiBroker.AddIngestionTrackingAsync(ingestionTracking);

                this.blobStorageBrokerMock.Setup(broker =>
                    broker.SelectByFileNameAsync(ingestionTracking.EncryptedFileName))
                        .ReturnsAsync(encryptedData);

                //When
                await this.apiBroker.DecryptFileAsync(randomFileName);

                //Then
                bool isDecrypted = await this.apiBroker.IsFileDecryptedAsync(randomFileName);

                isDecrypted.Should().BeTrue();

                var audits = this.apiBroker.RetrieveAllAudits()
                    .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

                foreach (var audit in audits)
                {
                    await this.apiBroker.RemoveAuditByIdAsync(audit.Id);
                }

                IngestionTracking decryptedIngestionTracking =
                    await this.apiBroker.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

                await this.apiBroker.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
            }

        }

        [Fact]
        public async Task ShouldNotDecryptNonExistentFileAsync()
        {
            // given
            string nonExistentFileName = GetRandomString();

            // when
            Func<Task> decryptFunc = async () => { await this.apiBroker.DecryptFileAsync(nonExistentFileName); };

            // then
            await decryptFunc.Should().ThrowAsync<DownloadOrchestrationValidationException>();
        }

        [Fact]
        public async Task ShouldThrowOnDecryptionServiceExceptionAsync()
        {
            // given
            string fileNameCausingServiceException = GetRandomString();

            // when
            Func<Task> decryptFunc = async () => { await this.apiBroker.DecryptFileAsync(fileNameCausingServiceException); };

            // then
            await decryptFunc.Should().ThrowAsync<DownloadOrchestrationServiceException>();
        }

        [Fact]
        public async Task ShouldThrowOnDecryptionDependencyExceptionAsync()
        {
            // given
            string fileNameCausingDependencyException = GetRandomString();

            // when
            Func<Task> decryptFunc = async () => { await this.apiBroker.DecryptFileAsync(fileNameCausingDependencyException); };

            // then
            await decryptFunc.Should().ThrowAsync<DownloadOrchestrationDependencyException>();
        }

       
    }

}
