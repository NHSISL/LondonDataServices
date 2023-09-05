// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataType;
using Microsoft.AspNetCore.Mvc;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DataTypesRelativeUrl = "api/dataTypes";

        public async ValueTask<DataType> PostDataTypeAsync(DataType dataType) =>
            await this.apiFactoryClient.PostContentAsync(DataTypesRelativeUrl, dataType);

        public async ValueTask<List<DataType>> GetAllDataTypesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<DataType>>($"{DataTypesRelativeUrl}/");

        public async ValueTask<DataType> GetDataTypeByIdAsync(Guid dataTypeId) =>
            await this.apiFactoryClient.GetContentAsync<DataType>($"{DataTypesRelativeUrl}/{dataTypeId}");

        public async ValueTask<DataType> PutDataTypeAsync(DataType ingestionTracking) =>
            await this.apiFactoryClient.PutContentAsync(DataTypesRelativeUrl, ingestionTracking);

        public async ValueTask<DataType> DeleteDataTypeByIdAsync(Guid dataTypeId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataType>(
                $"{DataTypesRelativeUrl}/{dataTypeId}");
    }
}
