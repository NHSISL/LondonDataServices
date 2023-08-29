using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public interface IObjectColumnService
    {
        ValueTask<ObjectColumn> AddObjectColumnAsync(ObjectColumn objectColumn);
        IQueryable<ObjectColumn> RetrieveAllObjectColumns();
    }
}