// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DataSetSpecificationsRelativeUrl = "api/DataSetSpecifications";

        public async ValueTask<DataSetSpecification> PostDataSetSpecificationAsync(
            DataSetSpecification DataSetSpecification) =>
                await this.apiFactoryClient.PostContentAsync(DataSetSpecificationsRelativeUrl, DataSetSpecification);

        public async ValueTask<List<DataSetSpecification>> GetAllDataSetSpecificationsAsync() =>
           await this.apiFactoryClient.GetContentAsync<List<DataSetSpecification>>(
               $"{DataSetSpecificationsRelativeUrl}/");

        public async ValueTask<DataSetSpecification> GetDataSetSpecificationByIdAsync(Guid DataSetSpecificationId) =>
            await this.apiFactoryClient.GetContentAsync<DataSetSpecification>(
                $"{DataSetSpecificationsRelativeUrl}/{DataSetSpecificationId}");

        public async ValueTask<DataSetSpecification> DeleteDataSetSpecificationByIdAsync(Guid DataSetSpecificationId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataSetSpecification>(
                $"{DataSetSpecificationsRelativeUrl}/{DataSetSpecificationId}");
    }
}
