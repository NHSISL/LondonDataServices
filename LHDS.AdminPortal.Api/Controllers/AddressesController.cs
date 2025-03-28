// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Services.Foundations.Addresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : RESTFulController
    {
        private readonly IAddressService addressService;

        public AddressesController(IAddressService addressService) =>
            this.addressService = addressService;

        [Authorize(Roles = "ISL.LDS.AdminApi.Addresses,ISL.LDS.AdminApi.Administrators")]
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

        [Authorize(Roles = "ISL.LDS.AdminApi.Addresses,ISL.LDS.AdminApi.Administrators,ISL.LDS.AdminApi.ReadOnly")]
        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.Address, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public ActionResult<IQueryable<Address>> Get()
        {
            try
            {
                IQueryable<Address> retrievedAddresses =
                    await this.addressService.RetrieveAllAddressesAsync();

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

        [Authorize(Roles = "ISL.LDS.AdminApi.Addresses,ISL.LDS.AdminApi.Administrators,ISL.LDS.AdminApi.ReadOnly")]
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

        [Authorize(Roles = "ISL.LDS.AdminApi.Addresses,ISL.LDS.AdminApi.Administrators")]
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

        [Authorize(Roles = "ISL.LDS.AdminApi.Addresses,ISL.LDS.AdminApi.Administrators")]
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
                return BadRequest(addressDependencyValidationException.InnerException);
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