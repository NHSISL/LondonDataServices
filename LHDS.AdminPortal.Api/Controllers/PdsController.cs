// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using LHDS.Core.Services.Foundations.PdsAudits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdsAuditsController : RESTFulController
    {
        private readonly IPdsAuditService pdsAuditService;

        public PdsAuditsController(IPdsAuditService pdsAuditService) =>
            this.pdsAuditService = pdsAuditService;

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Pds")]
#endif
        public async ValueTask<ActionResult<PdsAudit>> PostPdsAuditAsync(PdsAudit pdsAudit)
        {
            try
            {
                PdsAudit addedPdsAudit =
                    await this.pdsAuditService.AddPdsAuditAsync(pdsAudit);

                return Created(addedPdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is InvalidPdsAuditReferenceException)
            {
                return FailedDependency(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
               when (pdsAuditDependencyValidationException.InnerException is AlreadyExistsPdsAuditException)
            {
                return Conflict(pdsAuditDependencyValidationException.InnerException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpGet]
        [EnableQuery(PageSize = 50)]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Pds, lhdsApi.ReadOnly")]
#endif
        public ActionResult<IQueryable<PdsAudit>> Get()
        {
            try
            {
                IQueryable<PdsAudit> retrievedPdsAudits =
                    this.pdsAuditService.RetrieveAllPdsAudits();

                return Ok(retrievedPdsAudits);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpGet("{pdsAuditId}")]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Pds, lhdsApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<PdsAudit>> GetPdsAuditByIdAsync(Guid pdsAuditId)
        {
            try
            {
                PdsAudit pdsAudit =
                    await this.pdsAuditService.RetrievePdsAuditByIdAsync(pdsAuditId);

                return Ok(pdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is NotFoundPdsAuditException)
            {
                return NotFound(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpPut]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Pds")]
#endif
        public async ValueTask<ActionResult<PdsAudit>> PutPdsAuditAsync(PdsAudit pdsAudit)
        {
            try
            {
                PdsAudit modifiedPdsAudit =
                    await this.pdsAuditService.ModifyPdsAuditAsync(pdsAudit);

                return Ok(modifiedPdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is NotFoundPdsAuditException)
            {
                return NotFound(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is InvalidPdsAuditReferenceException)
            {
                return FailedDependency(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
               when (pdsAuditDependencyValidationException.InnerException is AlreadyExistsPdsAuditException)
            {
                return Conflict(pdsAuditDependencyValidationException.InnerException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpDelete("{pdsAuditId}")]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Pds")]
#endif
        public async ValueTask<ActionResult<PdsAudit>> DeletePdsAuditByIdAsync(Guid pdsAuditId)
        {
            try
            {
                PdsAudit deletedPdsAudit =
                    await this.pdsAuditService.RemovePdsAuditByIdAsync(pdsAuditId);

                return Ok(deletedPdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is NotFoundPdsAuditException)
            {
                return NotFound(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
                when (pdsAuditDependencyValidationException.InnerException is LockedPdsAuditException)
            {
                return Locked(pdsAuditDependencyValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
            {
                return BadRequest(pdsAuditDependencyValidationException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }
    }
}