// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string suppliersRelativeUrl = "api/suppliers";
        private const string suppliersRelativeOdataUrl = "odata/suppliers";

        public async ValueTask<Supplier> PostSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PostContentAsync(suppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> GetSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.GetContentAsync<Supplier>($"{suppliersRelativeUrl}/{supplierId}");

        public async ValueTask<List<Supplier>> GetAllSuppliersAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Supplier>>($"{suppliersRelativeUrl}/");

        public async ValueTask<List<Supplier>> FilterSuppliersAsync(string supplierName) =>
            await this.apiFactoryClient.GetContentAsync<List<Supplier>>(
                $"{suppliersRelativeUrl}/?$filter=name eq '{supplierName}'");

        public async ValueTask<Supplier> PutSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PutContentAsync(suppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> DeleteSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.DeleteContentAsync<Supplier>($"{suppliersRelativeUrl}/{supplierId}");
    }
}
