// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using LHDS.Core.Services.Processings.OptOuts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.OptOut")]
    public class OptOutsController : RESTFulController
    {
        private readonly IOptOutProcessingService optOutProcessingService;

        public OptOutsController(IOptOutProcessingService optOutProcessingService) =>
            this.optOutProcessingService = optOutProcessingService;

        [Authorize(Roles = "ISL.LDS.AdminSpa.OptOut,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        public async ValueTask<ActionResult<IQueryable<OptOut>>> Get()
        {
            try
            {
                IQueryable<OptOut> retrievedOptOuts =
                    await this.optOutProcessingService.RetrieveAllOptOutsAsync();

                return Ok(retrievedOptOuts);
            }
            catch (OptOutProcessingDependencyException optOutProcessingDependencyException)
            {
                return InternalServerError(optOutProcessingDependencyException);
            }
            catch (OptOutProcessingServiceException optOutProcessingServiceException)
            {
                return InternalServerError(optOutProcessingServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.OptOut")]
        [HttpPost]
        public async ValueTask<ActionResult<OptOut>> PostOptOutAsync(
            OptOut optOut)
        {
            try
            {
                OptOut addedOptOut =
                    await this.optOutProcessingService.RetrieveOrAddOptOutAsync(optOut);

                return Created(addedOptOut);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
            {
                return BadRequest(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is InvalidOptOutReferenceException)
            {
                return FailedDependency(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingDependencyValidationException)
               when (optOutProcessingDependencyValidationException.InnerException is AlreadyExistsOptOutException)
            {
                return Conflict(optOutProcessingDependencyValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyException optOutProcessingDependencyException)
            {
                return InternalServerError(optOutProcessingDependencyException);
            }
            catch (OptOutProcessingServiceException optOutProcessingServiceException)
            {
                return InternalServerError(optOutProcessingServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.OptOut,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet("{nhsNumber}")]
        public async ValueTask<ActionResult<OptOut>> GetOptOutByNhsNumberAsync(string nhsNumber)
        {
            try
            {
                OptOut getOptOutByNhs =
                    await this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(nhsNumber);

                return Ok(getOptOutByNhs);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is NotFoundOptOutException)
            {
                return NotFound(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
            {
                return BadRequest(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyException optOutProcessingDependencyException)
            {
                return InternalServerError(optOutProcessingDependencyException);
            }
            catch (OptOutProcessingServiceException optOutProcessingServiceException)
            {
                return InternalServerError(optOutProcessingServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.OptOut")]
        [HttpPut]
        public async ValueTask<ActionResult<OptOut>> PutOptOutAsync(OptOut optOut)
        {
            try
            {
                OptOut modifiedOptOut =
                    await this.optOutProcessingService.AddOrModifyOptOutAsync(optOut);

                return Ok(modifiedOptOut);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is NotFoundOptOutException)
            {
                return NotFound(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
            {
                return BadRequest(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is InvalidOptOutReferenceException)
            {
                return FailedDependency(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingDependencyValidationException)
               when (optOutProcessingDependencyValidationException.InnerException is AlreadyExistsOptOutException)
            {
                return Conflict(optOutProcessingDependencyValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyException optOutProcessingDependencyException)
            {
                return InternalServerError(optOutProcessingDependencyException);
            }
            catch (OptOutProcessingServiceException optOutProcessingServiceException)
            {
                return InternalServerError(optOutProcessingServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.OptOut")]
        [HttpDelete("{optOutId}")]
        public async ValueTask<ActionResult<OptOut>> DeleteOptOutByIdAsync(Guid optOutId)
        {
            try
            {
                OptOut deletedOptout =
                    await this.optOutProcessingService.RemoveOptOutByIdAsync(optOutId);

                return Ok(deletedOptout);
            }
            catch (OptOutValidationException optOutValidationException)
                 when (optOutValidationException.InnerException is NotFoundOptOutException)
            {
                return NotFound(optOutValidationException.InnerException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                return BadRequest(optOutValidationException.InnerException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
                when (optOutDependencyValidationException.InnerException is LockedOptOutException)
            {
                return Locked(optOutDependencyValidationException.InnerException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                return BadRequest(optOutDependencyValidationException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                return InternalServerError(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                return InternalServerError(optOutServiceException);
            }
        }
    }
}