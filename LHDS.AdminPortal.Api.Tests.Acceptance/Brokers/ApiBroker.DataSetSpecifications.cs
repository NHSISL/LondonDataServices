// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DataSetSpecificationsRelativeUrl = "api/DataSetSpecifications";

        public async ValueTask<DataSetSpecification> PostDataSetSpecificationAsync(DataSetSpecification ingestionTracking) =>
            await this.apiFactoryClient.PostContentAsync(DataSetSpecificationsRelativeUrl, ingestionTracking);

        public async ValueTask<DataSetSpecification> DeleteDataSetSpecificationByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataSetSpecification>($"{DataSetSpecificationsRelativeUrl}/{ingestionTrackingId}");
    }
}
