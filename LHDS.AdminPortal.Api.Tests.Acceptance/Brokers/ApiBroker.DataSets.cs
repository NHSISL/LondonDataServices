// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DataSetsRelativeUrl = "api/dataSets";
        private const string DataSetsRelativeOdataUrl = "odata/dataSets";

        public async ValueTask<DataSet> PostDataSetAsync(DataSet dataSet) =>
            await this.apiFactoryClient.PostContentAsync(DataSetsRelativeUrl, dataSet);

        public async ValueTask<List<DataSet>> GetAllDataSetsAsync()
        {
            OdataResponse<DataSet> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<DataSet>>($"{DataSetsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<DataSet> GetDataSetByIdAsync(Guid dataSetId) =>
            await this.apiFactoryClient.GetContentAsync<DataSet>($"{DataSetsRelativeUrl}/{dataSetId}");

        public async ValueTask<DataSet> PutDataSetAsync(DataSet dataSetId) =>
            await this.apiFactoryClient.PutContentAsync(DataSetsRelativeUrl, dataSetId);

        public async ValueTask<DataSet> DeleteDataSetByIdAsync(Guid dataSetId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataSet>($"{DataSetsRelativeUrl}/{dataSetId}");
    }
}
