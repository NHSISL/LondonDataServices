using System;
using System.Linq;
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

        [HttpGet]
        public ActionResult<IQueryable<IngestionTracking>> GetAllIngestionTrackings()
        {
            try
            {
                IQueryable<IngestionTracking> retrievedIngestionTrackings =
                    this.ingestionTrackingService.RetrieveAllIngestionTrackings();

                return Ok(retrievedIngestionTrackings);
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

        [HttpGet("{ingestionTrackingId}")]
        public async ValueTask<ActionResult<IngestionTracking>> GetIngestionTrackingByIdAsync(Guid ingestionTrackingId)
        {
            try
            {
                IngestionTracking ingestionTracking = await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId);

                return Ok(ingestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
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

        [HttpPut]
        public async ValueTask<ActionResult<IngestionTracking>> PutIngestionTrackingAsync(IngestionTracking ingestionTracking)
        {
            try
            {
                IngestionTracking modifiedIngestionTracking =
                    await this.ingestionTrackingService.ModifyIngestionTrackingAsync(ingestionTracking);

                return Ok(modifiedIngestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
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

        [HttpDelete("{ingestionTrackingId}")]
        public async ValueTask<ActionResult<IngestionTracking>> DeleteIngestionTrackingByIdAsync(Guid ingestionTrackingId)
        {
            try
            {
                IngestionTracking deletedIngestionTracking =
                    await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTrackingId);

                return Ok(deletedIngestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
                when (ingestionTrackingDependencyValidationException.InnerException is LockedIngestionTrackingException)
            {
                return Locked(ingestionTrackingDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                return BadRequest(ingestionTrackingDependencyValidationException);
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