using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public interface IAddressService
    {
        ValueTask<Address> AddAddressAsync(Address address);
        IQueryable<Address> RetrieveAllAddresses();
        ValueTask<Address> RetrieveAddressByIdAsync(Guid addressId);
        ValueTask<Address> ModifyAddressAsync(Address address);
        ValueTask<Address> RemoveAddressByIdAsync(Guid addressId);
    }
}