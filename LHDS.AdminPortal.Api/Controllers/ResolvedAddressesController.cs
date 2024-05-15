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
    }
}