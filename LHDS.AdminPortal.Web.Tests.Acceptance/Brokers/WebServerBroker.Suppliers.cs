// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Web.Tests.Acceptance.Models.Suppliers;
using Xunit;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Brokers
{
    public partial class WebServerBroker : IAsyncLifetime, IDisposable
    {
        private const string suppliersRelativeUrl = "api/suppliers";

        public async ValueTask<Supplier> PostSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PostContentAsync(suppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> GetSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.GetContentAsync<Supplier>($"{suppliersRelativeUrl}/{supplierId}");
        public async ValueTask<Supplier> PutSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PutContentAsync(suppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> DeleteSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.DeleteContentAsync<Supplier>($"{suppliersRelativeUrl}/{supplierId}");
    }
}
