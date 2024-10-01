using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects
{
    public interface ISpecificationObjectService
    {
        ValueTask<SpecificationObject> AddSpecificationObjectAsync(SpecificationObject specificationObject);
        IQueryable<SpecificationObject> RetrieveAllSpecificationObjects();
        ValueTask<SpecificationObject> RetrieveSpecificationObjectByIdAsync(Guid specificationObjectId);
        ValueTask<SpecificationObject> ModifySpecificationObjectAsync(SpecificationObject specificationObject);
        ValueTask<SpecificationObject> RemoveSpecificationObjectByIdAsync(Guid specificationObjectId);
    }
}