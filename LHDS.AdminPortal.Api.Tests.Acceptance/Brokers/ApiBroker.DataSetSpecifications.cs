// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string dataSetSpecificationsRelativeUrl = "api/dataSetSpecifications";
        private const string dataSetSpecificationsRelativeOdataUrl = "odata/dataSetSpecifications";

        public async ValueTask<DataSetSpecification> PostDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
                await this.apiFactoryClient.PostContentAsync(dataSetSpecificationsRelativeUrl, dataSetSpecification);

        public async ValueTask<List<DataSetSpecification>> GetAllDataSetSpecificationsAsync()
        {
            OdataResponse<DataSetSpecification> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<DataSetSpecification>>(
                    $"{dataSetSpecificationsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<DataSetSpecification> GetDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            await this.apiFactoryClient.GetContentAsync<DataSetSpecification>(
                $"{dataSetSpecificationsRelativeUrl}/{dataSetSpecificationId}");

        public async ValueTask<DataSetSpecification> PutDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
                await this.apiFactoryClient.PutContentAsync(dataSetSpecificationsRelativeUrl, dataSetSpecification);

        public async ValueTask<DataSetSpecification> DeleteDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataSetSpecification>(
                $"{dataSetSpecificationsRelativeUrl}/{dataSetSpecificationId}");
    }
}
