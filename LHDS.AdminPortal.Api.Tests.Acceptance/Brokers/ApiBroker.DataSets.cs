// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DataSetsRelativeUrl = "api/dataSets";

        public async ValueTask<DataSet> PostDataSetAsync(DataSet dataSet) =>
            await this.apiFactoryClient.PostContentAsync(DataSetsRelativeUrl, dataSet);

        public async ValueTask<List<DataSet>> GetAllDataSetsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<DataSet>>($"{DataSetsRelativeUrl}/");

        public async ValueTask<DataSet> GetDataSetByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.GetContentAsync<DataSet>($"{DataSetsRelativeUrl}/{ingestionTrackingId}");

        public async ValueTask<DataSet> DeleteDataSetByIdAsync(Guid dataSetId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataSet>($"{DataSetsRelativeUrl}/{dataSetId}");
    }
}
