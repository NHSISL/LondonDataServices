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
        private const string SuppliersRelativeUrl = "api/suppliers";

        public async ValueTask<Supplier> PostSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PostContentAsync(SuppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> GetSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.GetContentAsync<Supplier>($"{SuppliersRelativeUrl}/{supplierId}");

        public async ValueTask<List<Supplier>> GetAllSuppliersAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<Supplier>>($"{SuppliersRelativeUrl}/");

        public async ValueTask<List<Supplier>> FindSupplierByIdAsync(Guid supplierId) =>
          await this.apiFactoryClient.GetContentAsync<List<Supplier>>(
              $"{SuppliersRelativeUrl}/?$filter=Id eq {supplierId}");

        public async ValueTask<Supplier> PutSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PutContentAsync(SuppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> DeleteSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.DeleteContentAsync<Supplier>($"{SuppliersRelativeUrl}/{supplierId}");
    }
}
