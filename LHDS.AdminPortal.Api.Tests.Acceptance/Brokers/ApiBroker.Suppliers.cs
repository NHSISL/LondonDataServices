// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;
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

        public async ValueTask<List<Supplier>> GetAllSuppliersAsync()
        {
            OdataResponse<Supplier> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<Supplier>>($"{suppliersRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<List<Supplier>> FilterSuppliersAsync(string supplierName)
        {
            OdataResponse<Supplier> response = await this.apiFactoryClient.GetContentAsync<OdataResponse<Supplier>>(
                $"{suppliersRelativeOdataUrl}/?$filter=name eq '{supplierName}'");

            return response.Items;
        }

        public async ValueTask<List<Supplier>> GetAllSuppliersOrderedDescendingAsync()
        {
            OdataResponse<Supplier> response = await this.apiFactoryClient.GetContentAsync<OdataResponse<Supplier>>(
                $"{suppliersRelativeOdataUrl}/?$orderby=createddate desc");

            return response.Items;
        }

        public async ValueTask<List<Supplier>> GetAllSupplierIngestionTrackingExpandsAsync()
        {
            OdataResponse<Supplier> response = await this.apiFactoryClient.GetContentAsync<OdataResponse<Supplier>>(
                $"{suppliersRelativeOdataUrl}/" +
                    $"?$expand=ingestiontrackings($orderby=CreatedDate asc)&$orderby=createddate asc");

            return response.Items;
        }

        public async ValueTask<List<Supplier>> FindSupplierByIdAsync(Guid supplierId) =>
          await this.apiFactoryClient.GetContentAsync<List<Supplier>>(
              $"{suppliersRelativeUrl}/?$filter=Id eq {supplierId}");

        public async ValueTask<Supplier> PutSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PutContentAsync(suppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> DeleteSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.DeleteContentAsync<Supplier>($"{suppliersRelativeUrl}/{supplierId}");
    }
}
