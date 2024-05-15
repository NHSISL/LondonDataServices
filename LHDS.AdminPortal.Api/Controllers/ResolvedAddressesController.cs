using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.ResolvedAddresses;
using LHDS.AdminPortal.Api.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.ResolvedAddresses;

namespace LHDS.AdminPortal.Api.Controllers
{
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
               when (resolvedAddressDependencyValidationException.InnerException is AlreadyExistsResolvedAddressException)
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

        [HttpGet]
        public ActionResult<IQueryable<ResolvedAddress>> GetAllResolvedAddresses()
        {
            try
            {
                IQueryable<ResolvedAddress> retrievedResolvedAddresses =
                    this.resolvedAddressService.RetrieveAllResolvedAddresses();

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

        [HttpGet("{resolvedAddressId}")]
        public async ValueTask<ActionResult<ResolvedAddress>> GetResolvedAddressByIdAsync(Guid resolvedAddressId)
        {
            try
            {
                ResolvedAddress resolvedAddress = await this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddressId);

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
    }
}