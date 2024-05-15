using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.Addresses;
using LHDS.AdminPortal.Api.Models.Foundations.Addresses.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.Addresses;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : RESTFulController
    {
        private readonly IAddressService addressService;

        public AddressesController(IAddressService addressService) =>
            this.addressService = addressService;

        [HttpPost]
        public async ValueTask<ActionResult<Address>> PostAddressAsync(Address address)
        {
            try
            {
                Address addedAddress =
                    await this.addressService.AddAddressAsync(address);

                return Created(addedAddress);
            }
            catch (AddressValidationException addressValidationException)
            {
                return BadRequest(addressValidationException.InnerException);
            }
            catch (AddressDependencyValidationException addressValidationException)
                when (addressValidationException.InnerException is InvalidAddressReferenceException)
            {
                return FailedDependency(addressValidationException.InnerException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
               when (addressDependencyValidationException.InnerException is AlreadyExistsAddressException)
            {
                return Conflict(addressDependencyValidationException.InnerException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                return InternalServerError(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                return InternalServerError(addressServiceException);
            }
        }
    }
}