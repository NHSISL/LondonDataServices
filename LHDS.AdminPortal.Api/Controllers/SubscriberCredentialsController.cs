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
    }
}