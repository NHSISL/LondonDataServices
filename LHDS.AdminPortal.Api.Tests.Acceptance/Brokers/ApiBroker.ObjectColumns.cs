// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.ObjectColumns;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string objectColumnsRelativeUrl = "api/objectColumns";

        public async ValueTask<ObjectColumn> PostObjectColumnAsync(
            ObjectColumn objectColumn) =>
                await this.apiFactoryClient.PostContentAsync(objectColumnsRelativeUrl, objectColumn);

        public async ValueTask<ObjectColumn> DeleteObjectColumnByIdAsync(
            Guid objectColumnId) =>
                await this.apiFactoryClient.DeleteContentAsync<ObjectColumn>(
                    $"{objectColumnsRelativeUrl}/{objectColumnId}");
    }
}
