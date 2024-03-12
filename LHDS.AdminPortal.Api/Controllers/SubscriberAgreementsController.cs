using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.SubscriberAgreements;
using LHDS.AdminPortal.Api.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.SubscriberAgreements;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberAgreementsController : RESTFulController
    {
        private readonly ISubscriberAgreementService subscriberAgreementService;

        public SubscriberAgreementsController(ISubscriberAgreementService subscriberAgreementService) =>
            this.subscriberAgreementService = subscriberAgreementService;

        [HttpPost]
        public async ValueTask<ActionResult<SubscriberAgreement>> PostSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement)
        {
            try
            {
                SubscriberAgreement addedSubscriberAgreement =
                    await this.subscriberAgreementService.AddSubscriberAgreementAsync(subscriberAgreement);

                return Created(addedSubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
            {
                return BadRequest(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementValidationException)
                when (subscriberAgreementValidationException.InnerException is InvalidSubscriberAgreementReferenceException)
            {
                return FailedDependency(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementDependencyValidationException)
               when (subscriberAgreementDependencyValidationException.InnerException is AlreadyExistsSubscriberAgreementException)
            {
                return Conflict(subscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                return InternalServerError(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                return InternalServerError(subscriberAgreementServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<SubscriberAgreement>> GetAllSubscriberAgreements()
        {
            try
            {
                IQueryable<SubscriberAgreement> retrievedSubscriberAgreements =
                    this.subscriberAgreementService.RetrieveAllSubscriberAgreements();

                return Ok(retrievedSubscriberAgreements);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                return InternalServerError(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                return InternalServerError(subscriberAgreementServiceException);
            }
        }
    }
}