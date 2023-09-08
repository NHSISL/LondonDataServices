// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string dataSetObjectsRelativeUrl = "api/dataSetObjects";

        public async ValueTask<SpecificationObject> PostDataSetObjectAsync(
            SpecificationObject specificationObject) =>
                await this.apiFactoryClient.PostContentAsync(dataSetObjectsRelativeUrl, specificationObject);

        public async ValueTask<SpecificationObject> DeleteDataSetObjectByIdAsync(
            Guid specificationObjectId) =>
                await this.apiFactoryClient.DeleteContentAsync<SpecificationObject>(
                    $"{dataSetObjectsRelativeUrl}/{specificationObjectId}");
    }
}
