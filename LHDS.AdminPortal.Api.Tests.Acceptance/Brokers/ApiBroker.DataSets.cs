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
        private const string dataSetsRelativeUrl = "api/dataSets";
        private const string dataSetsRelativeOdataUrl = "odata/dataSets";

        public async ValueTask<DataSet> PostDataSetAsync(DataSet dataSet) =>
            await this.apiFactoryClient.PostContentAsync(dataSetsRelativeUrl, dataSet);

        public async ValueTask<List<DataSet>> GetAllDataSetsAsync()
        {
            OdataResponse<DataSet> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<DataSet>>($"{dataSetsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<DataSet> GetDataSetByIdAsync(Guid dataSetId) =>
            await this.apiFactoryClient.GetContentAsync<DataSet>($"{dataSetsRelativeUrl}/{dataSetId}");

        public async ValueTask<DataSet> PutDataSetAsync(DataSet dataSetId) =>
            await this.apiFactoryClient.PutContentAsync(dataSetsRelativeUrl, dataSetId);

        public async ValueTask<DataSet> DeleteDataSetByIdAsync(Guid dataSetId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataSet>($"{dataSetsRelativeUrl}/{dataSetId}");
    }
}
