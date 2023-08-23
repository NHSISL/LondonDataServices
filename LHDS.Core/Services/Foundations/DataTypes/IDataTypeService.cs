using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public interface IDataTypeService
    {
        ValueTask<DataType> AddDataTypeAsync(DataType dataType);
        IQueryable<DataType> RetrieveAllDataTypes();
    }
}