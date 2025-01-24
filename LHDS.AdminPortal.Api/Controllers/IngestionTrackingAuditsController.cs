// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Attrify.Attributes;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.IngestionTrackingAudit")]
    [ApiController]
    [Route("api/[controller]")]
    public class IngestionTrackingAuditsController : RESTFulController
    {
        private readonly IIngestionTrackingAuditService ingestionTrackingAuditService;

        public IngestionTrackingAuditsController(IIngestionTrackingAuditService ingestionTrackingAuditService) =>
            this.ingestionTrackingAuditService = ingestionTrackingAuditService;

        [InvisibleApi]
        [HttpPost]
        public async ValueTask<ActionResult<IngestionTrackingAudit>> PostAuditAsync(IngestionTrackingAudit audit)
        {
            try
            {
                IngestionTrackingAudit addedIngestionTrackingAudit =
                    await this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(audit);

                return Created(addedIngestionTrackingAudit);
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
            {
                return BadRequest(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException ingestionTrackingAuditValidationException)
                when (ingestionTrackingAuditValidationException.InnerException
                    is InvalidIngestionTrackingAuditReferenceException)
            {
                return FailedDependency(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException ingestionTrackingAuditDependencyValidationException)
                when (ingestionTrackingAuditDependencyValidationException.InnerException
                    is AlreadyExistsIngestionTrackingAuditException)
            {
                return Conflict(ingestionTrackingAuditDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [Authorize(Roles =
            "ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.IngestionTrackingAudit,ISL.LDS.AdminSpa.ReadOnly")]
        public ActionResult<IQueryable<IngestionTrackingAudit>> Get()
        {
            try
            {
                IQueryable<IngestionTrackingAudit> retrievedIngestionTrackingAudits =
                    this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAudits();

                return Ok(retrievedIngestionTrackingAudits);
            }
            catch (IngestionTrackingAuditDependencyException ingestionTrackingAuditDependencyException)
            {
                return InternalServerError(ingestionTrackingAuditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException ingestionTrackingAuditServiceException)
            {
                return InternalServerError(ingestionTrackingAuditServiceException);
            }
        }

        [InvisibleApi]
        [HttpGet("{ingestionTrackingAuditId}")]
        public async ValueTask<ActionResult<IngestionTrackingAudit>> GetAuditByIdAsync(Guid ingestionTrackingAuditId)
        {
            try
            {
                IngestionTrackingAudit ingestionTrackingAudit = await this.ingestionTrackingAuditService
                    .RetrieveIngestionTrackingAuditByIdAsync(ingestionTrackingAuditId);

                return Ok(ingestionTrackingAudit);
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
                when (ingestionTrackingAuditValidationException.InnerException is NotFoundIngestionTrackingAuditException)
            {
                return NotFound(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [InvisibleApi]
        [HttpPut]
        public async ValueTask<ActionResult<IngestionTrackingAudit>> PutAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit)
        {
            try
            {
                IngestionTrackingAudit modifiedAudit =
                    await this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(ingestionTrackingAudit);

                return Ok(modifiedAudit);
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
                when (ingestionTrackingAuditValidationException.InnerException
                    is NotFoundIngestionTrackingAuditException)
            {
                return NotFound(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
            {
                return BadRequest(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException ingestionTrackingAuditValidationException)
                when (ingestionTrackingAuditValidationException.InnerException
                    is InvalidIngestionTrackingAuditReferenceException)
            {
                return FailedDependency(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
                    when (ingestionTrackingAuditDependencyValidationException.InnerException
                        is AlreadyExistsIngestionTrackingAuditException)
            {
                return Conflict(ingestionTrackingAuditDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyException ingestionTrackingAuditDependencyException)
            {
                return InternalServerError(ingestionTrackingAuditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException ingestionTrackingAuditServiceException)
            {
                return InternalServerError(ingestionTrackingAuditServiceException);
            }
        }

        [HttpDelete("{ingestionTrackingAuditId}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<IngestionTrackingAudit>> DeleteAuditByIdAsync(Guid ingestionTrackingAuditId)
        {
            try
            {
                IngestionTrackingAudit deletedIngestionTrackingAudit =
                    await this.ingestionTrackingAuditService
                        .RemoveIngestionTrackingAuditByIdAsync(ingestionTrackingAuditId);

                return Ok(deletedIngestionTrackingAudit);
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
                when (ingestionTrackingAuditValidationException.InnerException
                    is NotFoundIngestionTrackingAuditException)
            {
                return NotFound(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
            {
                return BadRequest(ingestionTrackingAuditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
                    when (ingestionTrackingAuditDependencyValidationException.InnerException
                        is LockedIngestionTrackingAuditException)
            {
                return Locked(ingestionTrackingAuditDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException ingestionTrackingAuditDependencyValidationException)
            {
                return BadRequest(ingestionTrackingAuditDependencyValidationException);
            }
            catch (IngestionTrackingAuditDependencyException ingestionTrackingAuditDependencyException)
            {
                return InternalServerError(ingestionTrackingAuditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException ingestionTrackingAuditServiceException)
            {
                return InternalServerError(ingestionTrackingAuditServiceException);
            }
        }
    }
}