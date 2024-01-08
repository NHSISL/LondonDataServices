// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Workflows
{
    public partial class EmisLandingsApiTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            //Given
            Guid supplierId = this.apiBroker.landingConfiguration.LandingSupplierId;
            string fileName = GetRandomFileName();
            byte[] documentData = Encoding.UTF8.GetBytes(GetRandomString());
            Supplier landingSupplier = CreateRandomSupplier(supplierId, DateTimeOffset.Now);
            DataSet activeDataSet = CreateRandomDataSet(supplierId);
            DataSetSpecification activeDataSetSpecification = CreateRandomDataSetSpecification(activeDataSet);

            Document document = new Document
            {
                DocumentData = documentData,
                FileName = fileName
            };

            List<Document> documents = new List<Document> { document };
            await this.apiBroker.PostSupplierAsync(landingSupplier);
            await this.apiBroker.PostDataSetAsync(activeDataSet);
            await this.apiBroker.PostDataSetSpecificationAsync(activeDataSetSpecification);

            //When
            var actualStringList = await this.apiBroker.PostProcessDocumentsAsync();

            //Then
            foreach (var actualFile in actualStringList)
            {
                IngestionTracking ingestionTracking =
                    await this.apiBroker.FindIngestionTrackingByFileNameAsync(actualFile);

                ingestionTracking.Should().NotBeNull();

                var audits = await this.apiBroker
                    .FindIngestionTrackingAuditByIngestionTrackingIdAsync(ingestionTracking.Id);

                foreach (var audit in audits)
                {
                    await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(audit.Id);
                }

                await this.apiBroker.DeleteIngestionTrackingByIdAsync(ingestionTracking.Id);
                await this.apiBroker.DeleteDocumentByFileNameAsync(actualFile);
            }

            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(activeDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(landingSupplier.Id);
        }
    }
}