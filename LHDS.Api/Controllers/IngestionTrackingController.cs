// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngestionTrackingController : RESTFulController
    {
        private readonly IIngestionTrackingBroker ingestionTrackingService;

        [HttpGet]
        public ActionResult<IQueryable<IngestionTracking>> Get()
        {
            try
            {
                IQueryable<IngestionTracking> retrievedIngestionTrackings =
                    this.ingestionTrackingService.RetrieveAllIngestionTracking();

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
    }
}
