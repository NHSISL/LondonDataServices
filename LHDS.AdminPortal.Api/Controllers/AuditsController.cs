// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Services.Foundations.Audits;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditsController : RESTFulController
    {
        private readonly IAuditService auditService;

        public AuditsController(IAuditService auditService) =>
            this.auditService = auditService;

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhds.Api.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<Audit>> PostAuditAsync(Audit audit)
        {
            try
            {
                Audit addedAudit =
                    await this.auditService.AddAuditAsync(audit);

                return Created(addedAudit);
            }
            catch (AuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditValidationException)
                when (auditValidationException.InnerException is InvalidAuditReferenceException)
            {
                return FailedDependency(auditValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
               when (auditDependencyValidationException.InnerException is AlreadyExistsAuditException)
            {
                return Conflict(auditDependencyValidationException.InnerException);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpGet]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhds.Api.IngestionTracking, lhdsApi.ReadOnly")]
#endif
        public ActionResult<IQueryable<Audit>> GetAllAudits()
        {
            try
            {
                IQueryable<Audit> retrievedAudits =
                    this.auditService.RetrieveAllAudits();

                return Ok(retrievedAudits);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpGet("{auditId}")]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhds.Api.IngestionTracking, lhdsApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<Audit>> GetAuditByIdAsync(Guid auditId)
        {
            try
            {
                Audit audit = await this.auditService.RetrieveAuditByIdAsync(auditId);

                return Ok(audit);
            }
            catch (AuditValidationException auditValidationException)
                when (auditValidationException.InnerException is NotFoundAuditException)
            {
                return NotFound(auditValidationException.InnerException);
            }
            catch (AuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpPut]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhds.Api.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<Audit>> PutAuditAsync(Audit audit)
        {
            try
            {
                Audit modifiedAudit =
                    await this.auditService.ModifyAuditAsync(audit);

                return Ok(modifiedAudit);
            }
            catch (AuditValidationException auditValidationException)
                when (auditValidationException.InnerException is NotFoundAuditException)
            {
                return NotFound(auditValidationException.InnerException);
            }
            catch (AuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditValidationException)
                when (auditValidationException.InnerException is InvalidAuditReferenceException)
            {
                return FailedDependency(auditValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
               when (auditDependencyValidationException.InnerException is AlreadyExistsAuditException)
            {
                return Conflict(auditDependencyValidationException.InnerException);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpDelete("{auditId}")]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhds.Api.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<Audit>> DeleteAuditByIdAsync(Guid auditId)
        {
            try
            {
                Audit deletedAudit =
                    await this.auditService.RemoveAuditByIdAsync(auditId);

                return Ok(deletedAudit);
            }
            catch (AuditValidationException auditValidationException)
                when (auditValidationException.InnerException is NotFoundAuditException)
            {
                return NotFound(auditValidationException.InnerException);
            }
            catch (AuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
                when (auditDependencyValidationException.InnerException is LockedAuditException)
            {
                return Locked(auditDependencyValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
            {
                return BadRequest(auditDependencyValidationException);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }
    }
}