using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressService
    {
        private void ValidateAddressOnAdd(Address address)
        {
            ValidateAddressIsNotNull(address);
        }

        private static void ValidateAddressIsNotNull(Address address)
        {
            if (address is null)
            {
                throw new NullAddressException(message: "Address is null.");
            }
        }
    }
}