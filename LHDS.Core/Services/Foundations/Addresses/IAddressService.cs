using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public interface IAddressService
    {
        ValueTask<Address> AddAddressAsync(Address address);
    }
}