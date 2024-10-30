// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects
{
    internal class SpecificationObjectProcessingService : ISpecificationObjectProcessingService
    {
        private readonly ISpecificationObjectService specificationObjectService;

        public SpecificationObjectProcessingService(ISpecificationObjectService specificationObjectService)
        {
            this.specificationObjectService = specificationObjectService;
        }

        public async ValueTask<SpecificationObject> ReadOrInsertSpecificationObjectAsync(
            SpecificationObject specificationObject) =>
                throw new NotImplementedException();

        public ValueTask<List<SpecificationObject>> RetrieveAllSpecificationObjectsAsync() =>
            throw new NotImplementedException();
    }
}
