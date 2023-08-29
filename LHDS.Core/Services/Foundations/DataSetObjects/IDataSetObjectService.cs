using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetObjects;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public interface IDataSetObjectService
    {
        ValueTask<DataSetObject> AddDataSetObjectAsync(DataSetObject dataSetObject);
        IQueryable<DataSetObject> RetrieveAllDataSetObjects();
        ValueTask<DataSetObject> RetrieveDataSetObjectByIdAsync(Guid dataSetObjectId);
        ValueTask<DataSetObject> ModifyDataSetObjectAsync(DataSetObject dataSetObject);
    }
}