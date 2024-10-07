// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects
{
    public interface ISpecificationObjectService
    {
        ValueTask<SpecificationObject> AddSpecificationObjectAsync(SpecificationObject specificationObject);
        ValueTask<IQueryable<SpecificationObject>> RetrieveAllSpecificationObjects();
        ValueTask<SpecificationObject> RetrieveSpecificationObjectByIdAsync(Guid specificationObjectId);
        ValueTask<SpecificationObject> ModifySpecificationObjectAsync(SpecificationObject specificationObject);
        ValueTask<SpecificationObject> RemoveSpecificationObjectByIdAsync(Guid specificationObjectId);
    }
}