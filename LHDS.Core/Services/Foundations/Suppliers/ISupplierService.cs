using System.Threading.Tasks;
using LHDS.Core.Models.Suppliers;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public interface ISupplierService
    {
        ValueTask<Supplier> AddSupplierAsync(Supplier supplier);
    }
}