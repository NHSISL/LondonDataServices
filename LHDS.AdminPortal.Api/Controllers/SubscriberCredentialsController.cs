using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.SubscriberCredentials;
using LHDS.AdminPortal.Api.Models.Foundations.SubscriberCredentials.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.SubscriberCredentials;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberCredentialsController : RESTFulController
    {
        private readonly ISubscriberCredentialService subscriberCredentialService;

        public SubscriberCredentialsController(ISubscriberCredentialService subscriberCredentialService) =>
            this.subscriberCredentialService = subscriberCredentialService;

        [HttpPost]
        public async ValueTask<ActionResult<SubscriberCredential>> PostSubscriberCredentialAsync(SubscriberCredential subscriberCredential)
        {
            try
            {
                SubscriberCredential addedSubscriberCredential =
                    await this.subscriberCredentialService.AddSubscriberCredentialAsync(subscriberCredential);

                return Created(addedSubscriberCredential);
            }
            catch (SubscriberCredentialValidationException subscriberCredentialValidationException)
            {
                return BadRequest(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyValidationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is InvalidSubscriberCredentialReferenceException)
            {
                return FailedDependency(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyValidationException subscriberCredentialDependencyValidationException)
               when (subscriberCredentialDependencyValidationException.InnerException is AlreadyExistsSubscriberCredentialException)
            {
                return Conflict(subscriberCredentialDependencyValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyException subscriberCredentialDependencyException)
            {
                return InternalServerError(subscriberCredentialDependencyException);
            }
            catch (SubscriberCredentialServiceException subscriberCredentialServiceException)
            {
                return InternalServerError(subscriberCredentialServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<SubscriberCredential>> GetAllSubscriberCredentials()
        {
            try
            {
                IQueryable<SubscriberCredential> retrievedSubscriberCredentials =
                    this.subscriberCredentialService.RetrieveAllSubscriberCredentials();

                return Ok(retrievedSubscriberCredentials);
            }
            catch (SubscriberCredentialDependencyException subscriberCredentialDependencyException)
            {
                return InternalServerError(subscriberCredentialDependencyException);
            }
            catch (SubscriberCredentialServiceException subscriberCredentialServiceException)
            {
                return InternalServerError(subscriberCredentialServiceException);
            }
        }

        [HttpGet("{subscriberCredentialId}")]
        public async ValueTask<ActionResult<SubscriberCredential>> GetSubscriberCredentialByIdAsync(Guid subscriberCredentialId)
        {
            try
            {
                SubscriberCredential subscriberCredential = await this.subscriberCredentialService.RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId);

                return Ok(subscriberCredential);
            }
            catch (SubscriberCredentialValidationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is NotFoundSubscriberCredentialException)
            {
                return NotFound(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialValidationException subscriberCredentialValidationException)
            {
                return BadRequest(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyException subscriberCredentialDependencyException)
            {
                return InternalServerError(subscriberCredentialDependencyException);
            }
            catch (SubscriberCredentialServiceException subscriberCredentialServiceException)
            {
                return InternalServerError(subscriberCredentialServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<SubscriberCredential>> PutSubscriberCredentialAsync(SubscriberCredential subscriberCredential)
        {
            try
            {
                SubscriberCredential modifiedSubscriberCredential =
                    await this.subscriberCredentialService.ModifySubscriberCredentialAsync(subscriberCredential);

                return Ok(modifiedSubscriberCredential);
            }
            catch (SubscriberCredentialValidationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is NotFoundSubscriberCredentialException)
            {
                return NotFound(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialValidationException subscriberCredentialValidationException)
            {
                return BadRequest(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyValidationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is InvalidSubscriberCredentialReferenceException)
            {
                return FailedDependency(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyValidationException subscriberCredentialDependencyValidationException)
               when (subscriberCredentialDependencyValidationException.InnerException is AlreadyExistsSubscriberCredentialException)
            {
                return Conflict(subscriberCredentialDependencyValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyException subscriberCredentialDependencyException)
            {
                return InternalServerError(subscriberCredentialDependencyException);
            }
            catch (SubscriberCredentialServiceException subscriberCredentialServiceException)
            {
                return InternalServerError(subscriberCredentialServiceException);
            }
        }

        [HttpDelete("{subscriberCredentialId}")]
        public async ValueTask<ActionResult<SubscriberCredential>> DeleteSubscriberCredentialByIdAsync(Guid subscriberCredentialId)
        {
            try
            {
                SubscriberCredential deletedSubscriberCredential =
                    await this.subscriberCredentialService.RemoveSubscriberCredentialByIdAsync(subscriberCredentialId);

                return Ok(deletedSubscriberCredential);
            }
            catch (SubscriberCredentialValidationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is NotFoundSubscriberCredentialException)
            {
                return NotFound(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialValidationException subscriberCredentialValidationException)
            {
                return BadRequest(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyValidationException subscriberCredentialDependencyValidationException)
                when (subscriberCredentialDependencyValidationException.InnerException is LockedSubscriberCredentialException)
            {
                return Locked(subscriberCredentialDependencyValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyValidationException subscriberCredentialDependencyValidationException)
            {
                return BadRequest(subscriberCredentialDependencyValidationException);
            }
            catch (SubscriberCredentialDependencyException subscriberCredentialDependencyException)
            {
                return InternalServerError(subscriberCredentialDependencyException);
            }
            catch (SubscriberCredentialServiceException subscriberCredentialServiceException)
            {
                return InternalServerError(subscriberCredentialServiceException);
            }
        }
    }
}