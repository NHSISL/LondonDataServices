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
        private const string specificationObjectsRelativeUrl = "api/specificationObjects";

        public async ValueTask<SpecificationObject> PostSpecificationObjectAsync(
            SpecificationObject specificationObject) =>
                await this.apiFactoryClient.PostContentAsync(specificationObjectsRelativeUrl, specificationObject);

        public async ValueTask<SpecificationObject> GetSpecificationObjectByIdAsync(Guid specificationObjectId) =>
            await this.apiFactoryClient.GetContentAsync<SpecificationObject>(
                $"{specificationObjectsRelativeUrl}/{specificationObjectId}");

        public async ValueTask<SpecificationObject> DeleteSpecificationObjectByIdAsync(
            Guid specificationObjectId) =>
                await this.apiFactoryClient.DeleteContentAsync<SpecificationObject>(
                    $"{specificationObjectsRelativeUrl}/{specificationObjectId}");
    }
}
