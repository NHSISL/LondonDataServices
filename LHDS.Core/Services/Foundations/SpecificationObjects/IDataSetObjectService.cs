using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public interface IDataSetObjectService
    {
        ValueTask<SpecificationObject> AddDataSetObjectAsync(SpecificationObject dataSetObject);
        IQueryable<SpecificationObject> RetrieveAllDataSetObjects();
        ValueTask<SpecificationObject> RetrieveDataSetObjectByIdAsync(Guid dataSetObjectId);
        ValueTask<SpecificationObject> ModifyDataSetObjectAsync(SpecificationObject dataSetObject);
        ValueTask<SpecificationObject> RemoveDataSetObjectByIdAsync(Guid dataSetObjectId);
    }
}