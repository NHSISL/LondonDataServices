// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Integration.TppLandings
{
    public partial class TppLandingTests
    {
        [Fact]
        public async Task ShouldLandTPPFileAsync()
        {
            // given
            byte[] fileBytes = File.ReadAllBytes(@"Resources\TppLandingTests\ShouldLandTPPFileAsync.txt");
            Stream fileStream = new MemoryStream(fileBytes);
            FileInfo fi = new FileInfo(@"Resources\TppLandingTests\ShouldLandTPPFileAsync.txt");
            var fileNameWithoutExtension = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            string sha256Hash = CalculateSHA256Hash(fileBytes);

            Document document = new Document
            {
                FileName = fileNameWithoutExtension,
                DocumentData = fileStream,
                SHA256Hash = sha256Hash
            };

            Supplier supplier = await SetupSupplier();
            DataSet dataSet = await SetupDataSet(supplier.Id);
            DataSetSpecification dataSetSpecification = await SetupDataSetSpecification(dataSet.Id);

            // when
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(document, supplier.Id);

            // then
            actualGuid.Should().NotBe(Guid.Empty);

            IngestionTracking actualIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            var audits = this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == actualGuid);

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.dataSetSpecificationService
               .RemoveDataSetSpecificationByIdAsync(dataSetSpecification.Id);

            await this.dataSetService.RemoveDataSetByIdAsync(dataSet.Id);
            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(actualGuid);
            await this.supplierService.RemoveSupplierByIdAsync(supplier.Id);
        }
    }
}
