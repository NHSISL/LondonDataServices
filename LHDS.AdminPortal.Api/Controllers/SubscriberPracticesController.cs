// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberPractices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberPracticesController : RESTFulController
    {
        private readonly ISubscriberPracticeService subscriberAgreementService;

        public SubscriberPracticesController(ISubscriberPracticeService subscriberAgreementService) =>
            this.subscriberAgreementService = subscriberAgreementService;

        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
        [HttpPost]
        public async ValueTask<ActionResult<SubscriberPractice>> PostSubscriberPracticeAsync(SubscriberPractice subscriberAgreement)
        {
            try
            {
                SubscriberPractice addedSubscriberPractice =
                    await this.subscriberAgreementService.AddSubscriberPracticeAsync(subscriberAgreement);

                return Created(addedSubscriberPractice);
            }
            catch (SubscriberPracticeValidationException subscriberAgreementValidationException)
            {
                return BadRequest(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberAgreementValidationException)
                when (subscriberAgreementValidationException.InnerException is InvalidSubscriberPracticeReferenceException)
            {
                return FailedDependency(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberAgreementDependencyValidationException)
               when (subscriberAgreementDependencyValidationException.InnerException is AlreadyExistsSubscriberPracticeException)
            {
                return Conflict(subscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyException subscriberAgreementDependencyException)
            {
                return InternalServerError(subscriberAgreementDependencyException);
            }
            catch (SubscriberPracticeServiceException subscriberAgreementServiceException)
            {
                return InternalServerError(subscriberAgreementServiceException);
            }
        }
    }
}