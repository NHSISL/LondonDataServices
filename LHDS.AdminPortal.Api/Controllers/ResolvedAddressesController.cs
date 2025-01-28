// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Configurations")]
    [ApiController]
    [Route("api/[controller]")]
    public class ResolvedAddressesController : RESTFulController
    {
        private readonly IResolvedAddressService resolvedAddressService;

        public ResolvedAddressesController(IResolvedAddressService resolvedAddressService) =>
            this.resolvedAddressService = resolvedAddressService;

        [HttpPost]
        public async ValueTask<ActionResult<ResolvedAddress>> PostResolvedAddressAsync(ResolvedAddress resolvedAddress)
        {
            try
            {
                ResolvedAddress addedResolvedAddress =
                    await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);

                return Created(addedResolvedAddress);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                return BadRequest(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressValidationException)
                when (resolvedAddressValidationException.InnerException is InvalidResolvedAddressReferenceException)
            {
                return FailedDependency(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
                when (resolvedAddressDependencyValidationException.InnerException
                    is AlreadyExistsResolvedAddressException)
            {
                return Conflict(resolvedAddressDependencyValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                return InternalServerError(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                return InternalServerError(resolvedAddressServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.ResolvedAddress, ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<ResolvedAddress>>> GetAllResolvedAddressesAsync()
        {
            try
            {
                IQueryable<ResolvedAddress> retrievedResolvedAddresses =
                    await this.resolvedAddressService.RetrieveAllResolvedAddressesAsync();

                return Ok(retrievedResolvedAddresses);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                return InternalServerError(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                return InternalServerError(resolvedAddressServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.ResolvedAddress, ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet("{resolvedAddressId}")]
        public async ValueTask<ActionResult<ResolvedAddress>> GetResolvedAddressByIdAsync(Guid resolvedAddressId)
        {
            try
            {
                ResolvedAddress resolvedAddress = await this.resolvedAddressService
                    .RetrieveResolvedAddressByIdAsync(resolvedAddressId);

                return Ok(resolvedAddress);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
                when (resolvedAddressValidationException.InnerException is NotFoundResolvedAddressException)
            {
                return NotFound(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                return BadRequest(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                return InternalServerError(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                return InternalServerError(resolvedAddressServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.ResolvedAddress")]
        [HttpPut]
        public async ValueTask<ActionResult<ResolvedAddress>> PutResolvedAddressAsync(ResolvedAddress resolvedAddress)
        {
            try
            {
                ResolvedAddress modifiedResolvedAddress =
                    await this.resolvedAddressService.ModifyResolvedAddressAsync(resolvedAddress);

                return Ok(modifiedResolvedAddress);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
                when (resolvedAddressValidationException.InnerException is NotFoundResolvedAddressException)
            {
                return NotFound(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                return BadRequest(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressValidationException)
                when (resolvedAddressValidationException.InnerException is InvalidResolvedAddressReferenceException)
            {
                return FailedDependency(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
                when (resolvedAddressDependencyValidationException.InnerException
                    is AlreadyExistsResolvedAddressException)
            {
                return Conflict(resolvedAddressDependencyValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                return InternalServerError(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                return InternalServerError(resolvedAddressServiceException);
            }
        }

        [HttpDelete("{resolvedAddressId}")]
        public async ValueTask<ActionResult<ResolvedAddress>> DeleteResolvedAddressByIdAsync(Guid resolvedAddressId)
        {
            try
            {
                ResolvedAddress deletedResolvedAddress =
                    await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(resolvedAddressId);

                return Ok(deletedResolvedAddress);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
                when (resolvedAddressValidationException.InnerException is NotFoundResolvedAddressException)
            {
                return NotFound(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                return BadRequest(resolvedAddressValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
                when (resolvedAddressDependencyValidationException.InnerException is LockedResolvedAddressException)
            {
                return Locked(resolvedAddressDependencyValidationException.InnerException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                return BadRequest(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                return InternalServerError(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                return InternalServerError(resolvedAddressServiceException);
            }
        }
    }
}