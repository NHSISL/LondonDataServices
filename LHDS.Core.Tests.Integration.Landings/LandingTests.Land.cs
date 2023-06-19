// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Integration.Landings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldLandFilesAsync()
        {
            try
            {
                // given
                Supplier supplier = await SetupSupplier();

                // when
                List<string> files = await landingClient.ProcessAsync();

                // then
                files.Should().NotBeNull();

                foreach (var file in files)
                {
                    IngestionTracking ingestionTracking =
                        ingestionTrackingService.RetrieveAllIngestionTrackings()
                            .FirstOrDefault(item => item.DecryptedFileName == file);

                    ingestionTracking.Should().NotBeNull();

                    List<Audit> audits = auditService.RetrieveAllAudits()
                        .Where(item => item.IngestionTrackingId == ingestionTracking.Id).ToList();

                    foreach (Audit item in audits)
                    {
                        await auditService.RemoveAuditByIdAsync(item.Id);
                    }

                    await ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
                    await blobStorageBroker.DeleteFileAsync(ingestionTracking.EncryptedFileName);
                    await blobStorageBroker.DeleteFileAsync(ingestionTracking.DecryptedFileName);
                }
            }
            catch (Exception ex)
            {
                loggingBroker.LogError(ex);
                Console.WriteLine(ex.Message);
                Assert.Fail($"{ex.Message}, {ex.InnerException.Message}");
            }
        }
    }
}
