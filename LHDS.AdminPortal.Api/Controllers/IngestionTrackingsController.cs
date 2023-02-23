using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Models.IngestionTrackings.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.IngestionTrackings;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngestionTrackingsController : RESTFulController
    {
        private readonly IIngestionTrackingService ingestionTrackingService;

        public IngestionTrackingsController(IIngestionTrackingService ingestionTrackingService) =>
            this.ingestionTrackingService = ingestionTrackingService;

        [HttpPost]
        public async ValueTask<ActionResult<IngestionTracking>> PostIngestionTrackingAsync(IngestionTracking ingestionTracking)
        {
            try
            {
                IngestionTracking addedIngestionTracking =
                    await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

                return Created(addedIngestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is InvalidIngestionTrackingReferenceException)
            {
                return FailedDependency(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
               when (ingestionTrackingDependencyValidationException.InnerException is AlreadyExistsIngestionTrackingException)
            {
                return Conflict(ingestionTrackingDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }
    }
}