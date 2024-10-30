// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects
{
    public interface ISpecificationObjectProcessingService
    {
        ValueTask<SpecificationObject> ReadOrInsertSpecificationObjectAsync(SpecificationObject specificationObject);
        ValueTask<IQueryable<SpecificationObject>> RetrieveAllSpecificationObjectsAsync();
    }
}
