using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetObjects;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public interface IDataSetObjectService
    {
        ValueTask<DataSetObject> AddDataSetObjectAsync(DataSetObject dataSetObject);
    }
}