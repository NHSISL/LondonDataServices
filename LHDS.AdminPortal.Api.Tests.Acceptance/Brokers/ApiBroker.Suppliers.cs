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
            OdataResponce<Supplier> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponce<Supplier>>($"{suppliersRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<List<Supplier>> FilterSuppliersAsync(string supplierName)
        {
            OdataResponce<Supplier> response = await this.apiFactoryClient.GetContentAsync<OdataResponce<Supplier>>(
                $"{suppliersRelativeOdataUrl}/?$filter=name eq '{supplierName}'");

            return response.Items;
        }

        public async ValueTask<List<Supplier>> GetAllSuppliersOrderedDescendingAsync()
        {
            OdataResponce<Supplier> response = await this.apiFactoryClient.GetContentAsync<OdataResponce<Supplier>>(
                $"{suppliersRelativeOdataUrl}/?$orderby=createddate desc");

            return response.Items;
        }

        public async ValueTask<List<Supplier>> GetAllSupplierIngestionTrackingExpandsAsync()
        {
            OdataResponce<Supplier> response = await this.apiFactoryClient.GetContentAsync<OdataResponce<Supplier>>(
                $"{suppliersRelativeOdataUrl}/" +
                    $"?$expand=ingestiontrackings($orderby=CreatedDate asc)&$orderby=createddate asc");

            return response.Items;
        }

        public async ValueTask<Supplier> PutSupplierAsync(Supplier supplier) =>
            await this.apiFactoryClient.PutContentAsync(suppliersRelativeUrl, supplier);

        public async ValueTask<Supplier> DeleteSupplierByIdAsync(Guid supplierId) =>
            await this.apiFactoryClient.DeleteContentAsync<Supplier>($"{suppliersRelativeUrl}/{supplierId}");
    }
}
