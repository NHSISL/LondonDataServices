// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataType;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string dataTypesRelativeUrl = "api/dataTypes";
        private const string dataTypesRelativeOdataUrl = "odata/dataTypes";

        public async ValueTask<DataType> PostDataTypeAsync(DataType dataType) =>
            await this.apiFactoryClient.PostContentAsync(dataTypesRelativeUrl, dataType);

        public async ValueTask<List<DataType>> GetAllDataTypesAsync()
        {
            OdataResponse<DataType> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<DataType>>($"{dataTypesRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<DataType> GetDataTypeByIdAsync(Guid dataTypeId) =>
            await this.apiFactoryClient.GetContentAsync<DataType>($"{dataTypesRelativeUrl}/{dataTypeId}");

        public async ValueTask<DataType> PutDataTypeAsync(DataType ingestionTracking) =>
            await this.apiFactoryClient.PutContentAsync(dataTypesRelativeUrl, ingestionTracking);

        public async ValueTask<DataType> DeleteDataTypeByIdAsync(Guid dataTypeId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataType>(
                $"{dataTypesRelativeUrl}/{dataTypeId}");
    }
}
