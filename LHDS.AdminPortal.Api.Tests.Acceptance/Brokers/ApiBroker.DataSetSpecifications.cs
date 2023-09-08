// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DataSetSpecificationsRelativeUrl = "api/dataSetSpecifications";

        public async ValueTask<DataSetSpecification> PostDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
                await this.apiFactoryClient.PostContentAsync(DataSetSpecificationsRelativeUrl, dataSetSpecification);

        public async ValueTask<List<DataSetSpecification>> GetAllDataSetSpecificationsAsync() =>
           await this.apiFactoryClient.GetContentAsync<List<DataSetSpecification>>(
               $"{DataSetSpecificationsRelativeUrl}/");

        public async ValueTask<DataSetSpecification> GetDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            await this.apiFactoryClient.GetContentAsync<DataSetSpecification>(
                $"{DataSetSpecificationsRelativeUrl}/{dataSetSpecificationId}");

        public async ValueTask<DataSetSpecification> PutDataSetSpecificationAsync(
        DataSetSpecification dataSetSpecification) =>
                await this.apiFactoryClient.PutContentAsync(DataSetSpecificationsRelativeUrl, dataSetSpecification);

        public async ValueTask<DataSetSpecification> DeleteDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            await this.apiFactoryClient.DeleteContentAsync<DataSetSpecification>(
                $"{DataSetSpecificationsRelativeUrl}/{dataSetSpecificationId}");
    }
}
