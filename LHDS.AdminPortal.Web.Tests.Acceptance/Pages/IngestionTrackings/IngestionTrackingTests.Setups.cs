// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Web.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Web.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Web.Tests.Acceptance.Models.Suppliers;
using Microsoft.Playwright;
using Tynamix.ObjectFiller;
using Xunit;
using static LHDS.AdminPortal.Web.Tests.Acceptance.Pages.CommonFunctions;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Pages.IngestionTrackings
{
    public partial class IngestionTrackingTests : IClassFixture<WebServerBroker>
    {
        private async ValueTask<IPage> InitialiseSetup()
        {
            var context = await webServerBroker.browser.NewContextAsync(new() { IgnoreHTTPSErrors = true });

            var apiPage = await context.NewPageAsync();
            var page = await context.NewPageAsync();

            await Task.WhenAll(
                apiPage.GotoAsync(webServerBroker.ApiProxyBaseUrl),
                page.GotoAsync(webServerBroker.FrontendProxyBaseUrl)
                );

            return page;
        }

        private static async Task PerformLogin(IPage page)
        {
            var loginHandler = new Login(page);
            await loginHandler.PerformLogin();
        }

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(Guid supplierId)
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(supplierId);
            await this.webServerBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private static IngestionTracking CreateRandomIngestionTracking(Guid supplierId) =>
           CreateRandomIngestionTrackingFiller(supplierId).Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<Supplier> PostRandomSupplierAsync()
        {
            Supplier randomSupplier = CreateRandomSupplier();
            await this.webServerBroker.PostSupplierAsync(randomSupplier);

            return randomSupplier;
        }

        private static Supplier CreateRandomSupplier() =>
            CreateRandomSupplierFiller().Create();

        private static Filler<Supplier> CreateRandomSupplierFiller()
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId)
                .OnProperty(supplier => supplier.IngestionTrackings).IgnoreIt();

            return filler;
        }
    }

}
