// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.ObjectColumns;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string objectColumnsRelativeUrl = "api/objectColumns";
        private const string objectColumnsRelativeOdataUrl = "odata/objectColumns";

        public async ValueTask<ObjectColumn> PostObjectColumnAsync(
            ObjectColumn objectColumn) =>
                await this.apiFactoryClient.PostContentAsync(objectColumnsRelativeUrl, objectColumn);

        public async ValueTask<List<ObjectColumn>> GetAllObjectColumnsAsync()
        {
            OdataResponse<ObjectColumn> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<ObjectColumn>>($"{objectColumnsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<ObjectColumn> GetObjectColumnByIdAsync(Guid objectColumnId) =>
            await this.apiFactoryClient.GetContentAsync<ObjectColumn>(
                $"{objectColumnsRelativeUrl}/{objectColumnId}");

        public async ValueTask<ObjectColumn> PutObjectColumnAsync(ObjectColumn objectColumn) => 
            await this.apiFactoryClient.PutContentAsync(objectColumnsRelativeUrl, objectColumn);

        public async ValueTask<ObjectColumn> DeleteObjectColumnByIdAsync(
            Guid objectColumnId) =>
                await this.apiFactoryClient.DeleteContentAsync<ObjectColumn>(
                    $"{objectColumnsRelativeUrl}/{objectColumnId}");
    }
}
