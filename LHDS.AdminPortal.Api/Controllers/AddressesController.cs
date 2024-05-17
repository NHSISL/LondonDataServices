// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Services.Foundations.Addresses;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

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

        [HttpGet]
        public ActionResult<IQueryable<Address>> GetAllAddresses()
        {
            try
            {
                IQueryable<Address> retrievedAddresses =
                    this.addressService.RetrieveAllAddresses();

                return Ok(retrievedAddresses);
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

        [HttpGet("{addressId}")]
        public async ValueTask<ActionResult<Address>> GetAddressByIdAsync(Guid addressId)
        {
            try
            {
                Address address = await this.addressService.RetrieveAddressByIdAsync(addressId);

                return Ok(address);
            }
            catch (AddressValidationException addressValidationException)
                when (addressValidationException.InnerException is NotFoundAddressException)
            {
                return NotFound(addressValidationException.InnerException);
            }
            catch (AddressValidationException addressValidationException)
            {
                return BadRequest(addressValidationException.InnerException);
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

        [HttpPut]
        public async ValueTask<ActionResult<Address>> PutAddressAsync(Address address)
        {
            try
            {
                Address modifiedAddress =
                    await this.addressService.ModifyAddressAsync(address);

                return Ok(modifiedAddress);
            }
            catch (AddressValidationException addressValidationException)
                when (addressValidationException.InnerException is NotFoundAddressException)
            {
                return NotFound(addressValidationException.InnerException);
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

        [HttpDelete("{addressId}")]
        public async ValueTask<ActionResult<Address>> DeleteAddressByIdAsync(Guid addressId)
        {
            try
            {
                Address deletedAddress =
                    await this.addressService.RemoveAddressByIdAsync(addressId);

                return Ok(deletedAddress);
            }
            catch (AddressValidationException addressValidationException)
                when (addressValidationException.InnerException is NotFoundAddressException)
            {
                return NotFound(addressValidationException.InnerException);
            }
            catch (AddressValidationException addressValidationException)
            {
                return BadRequest(addressValidationException.InnerException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
                when (addressDependencyValidationException.InnerException is LockedAddressException)
            {
                return Locked(addressDependencyValidationException.InnerException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                return BadRequest(addressDependencyValidationException);
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