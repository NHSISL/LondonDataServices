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
        private readonly ISubscriberPracticeService subscriberPracticeService;

        public SubscriberPracticesController(ISubscriberPracticeService subscriberPracticeService) =>
            this.subscriberPracticeService = subscriberPracticeService;

        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
        [HttpPost]
        public async ValueTask<ActionResult<SubscriberPractice>> PostSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice)
        {
            try
            {
                SubscriberPractice addedSubscriberPractice =
                    await this.subscriberPracticeService.AddSubscriberPracticeAsync(subscriberPractice);

                return Created(addedSubscriberPractice);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
            {
                return BadRequest(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeValidationException)
                when (subscriberPracticeValidationException.InnerException is 
                InvalidSubscriberPracticeReferenceException)
            {
                return FailedDependency(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeDependencyValidationException)
               when (subscriberPracticeDependencyValidationException.InnerException is 
               AlreadyExistsSubscriberPracticeException)
            {
                return Conflict(subscriberPracticeDependencyValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyException subscriberPracticeDependencyException)
            {
                return InternalServerError(subscriberPracticeDependencyException);
            }
            catch (SubscriberPracticeServiceException subscriberPracticeServiceException)
            {
                return InternalServerError(subscriberPracticeServiceException);
            }
        }
    }
}