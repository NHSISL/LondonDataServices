// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Services.Foundations.ResolvedAddressAudits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
    [ApiController]
    [Route("api/[controller]")]
    public class ResolvedAddressesAuditController : RESTFulController
    {
        private readonly IResolvedAddressAuditService resolvedAddressAuditService;

        public ResolvedAddressesAuditController(IResolvedAddressAuditService resolvedAddressAuditService) =>
            this.resolvedAddressAuditService = resolvedAddressAuditService;

        [HttpPost]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ResolvedAddress")]
        public async ValueTask<ActionResult<ResolvedAddressAudit>> PostResolvedAddressAuditAsync(
            ResolvedAddressAudit resolvedAddressAudit)
        {
            try
            {
                ResolvedAddressAudit addedResolvedAddressAudit =
                    await this.resolvedAddressAuditService.AddResolvedAddressAuditAsync(resolvedAddressAudit);

                return Created(addedResolvedAddressAudit);
            }
            catch (ResolvedAddressAuditValidationException resolvedAddressAuditValidationException)
            {
                return BadRequest(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyValidationException resolvedAddressAuditValidationException)
                when (resolvedAddressAuditValidationException.InnerException
                    is InvalidResolvedAddressAuditReferenceException)
            {
                return FailedDependency(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyValidationException resolvedAddressAuditDependencyValidationException)
                when (resolvedAddressAuditDependencyValidationException.InnerException
                is AlreadyExistsResolvedAddressAuditException)
            {
                return Conflict(resolvedAddressAuditDependencyValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyException resolvedAddressAuditDependencyException)
            {
                return InternalServerError(resolvedAddressAuditDependencyException);
            }
            catch (ResolvedAddressAuditServiceException resolvedAddressAuditServiceException)
            {
                return InternalServerError(resolvedAddressAuditServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
    [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ResolvedAddress, ISL.LDS.AdminApi.ReadOnly")]
        public async ValueTask<ActionResult<IQueryable<ResolvedAddressAudit>>> Get()
        {
            try
            {
                IQueryable<ResolvedAddressAudit> retrievedResolvedAddressAudits =
                    await this.resolvedAddressAuditService.RetrieveAllResolvedAddressAuditsAsync();

                return Ok(retrievedResolvedAddressAudits);
            }
            catch (ResolvedAddressAuditDependencyException resolvedAddressAuditDependencyException)
            {
                return InternalServerError(resolvedAddressAuditDependencyException);
            }
            catch (ResolvedAddressAuditServiceException resolvedAddressAuditServiceException)
            {
                return InternalServerError(resolvedAddressAuditServiceException);
            }
        }

        [HttpGet("{resolvedAddressAuditId}")]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ResolvedAddress, ISL.LDS.AdminApi.ReadOnly")]
        public async ValueTask<ActionResult<ResolvedAddressAudit>> GetResolvedAddressAuditByIdAsync(
            Guid resolvedAddressAuditId)
        {
            try
            {
                ResolvedAddressAudit resolvedAddressAudit =
                    await this.resolvedAddressAuditService.RetrieveResolvedAddressAuditByIdAsync(
                        resolvedAddressAuditId);

                return Ok(resolvedAddressAudit);
            }

            catch (ResolvedAddressAuditValidationException resolvedAddressAuditValidationException)
                when (resolvedAddressAuditValidationException.InnerException
                is NotFoundResolvedAddressAuditException)
            {
                return NotFound(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditValidationException resolvedAddressAuditValidationException)
            {
                return BadRequest(resolvedAddressAuditValidationException.InnerException);
            }
            catch (ResolvedAddressAuditDependencyException resolvedAddressAuditDependencyException)
            {
                return InternalServerError(resolvedAddressAuditDependencyException);
            }
            catch (ResolvedAddressAuditServiceException resolvedAddressAuditServiceException)
            {
                return InternalServerError(resolvedAddressAuditServiceException);
            }
        }

        [HttpPut]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ResolvedAddress")]
        public async ValueTask<ActionResult<ResolvedAddressAudit>> PutResolvedAddressAuditAsync(
            ResolvedAddressAudit resolvedAddressAudit)
        {
            try
            {
                ResolvedAddressAudit modifiedResolvedAddressAudit =
                    await this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(resolvedAddressAudit);

                return Ok(modifiedResolvedAddressAudit);
            }

            catch (ResolvedAddressAuditValidationException resolvedAddressAuditValidationException)
                when (resolvedAddressAuditValidationException.InnerException
                    is NotFoundResolvedAddressAuditException)
            {
                return NotFound(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditValidationException resolvedAddressAuditValidationException)
            {
                return BadRequest(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyValidationException resolvedAddressAuditValidationException)
                when (resolvedAddressAuditValidationException.InnerException
                    is InvalidResolvedAddressAuditReferenceException)
            {
                return FailedDependency(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyValidationException resolvedAddressAuditDependencyValidationException)
                when (resolvedAddressAuditDependencyValidationException.InnerException
                is AlreadyExistsResolvedAddressAuditException)
            {
                return Conflict(resolvedAddressAuditDependencyValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyException resolvedAddressAuditDependencyException)
            {
                return InternalServerError(resolvedAddressAuditDependencyException);
            }
            catch (ResolvedAddressAuditServiceException resolvedAddressAuditServiceException)
            {
                return InternalServerError(resolvedAddressAuditServiceException);
            }
        }

        [HttpDelete("{resolvedAddressAuditId}")]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ResolvedAddress")]
        public async ValueTask<ActionResult<ResolvedAddressAudit>> DeleteResolvedAddressAuditByIdAsync(
            Guid resolvedAddressAuditId)
        {
            try
            {
                ResolvedAddressAudit deletedResolvedAddressAudit =
                    await this.resolvedAddressAuditService.RemoveResolvedAddressAuditByIdAsync(
                        resolvedAddressAuditId);

                return Ok(deletedResolvedAddressAudit);
            }

            catch (ResolvedAddressAuditValidationException resolvedAddressAuditValidationException)
                when (resolvedAddressAuditValidationException.InnerException
                    is NotFoundResolvedAddressAuditException)
            {
                return NotFound(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditValidationException resolvedAddressAuditValidationException)
            {
                return BadRequest(resolvedAddressAuditValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyValidationException resolvedAddressAuditDependencyValidationException)
                when (resolvedAddressAuditDependencyValidationException.InnerException
                    is LockedResolvedAddressAuditException)
            {
                return Locked(resolvedAddressAuditDependencyValidationException.InnerException);
            }

            catch (ResolvedAddressAuditDependencyValidationException resolvedAddressAuditDependencyValidationException)
            {
                return BadRequest(resolvedAddressAuditDependencyValidationException.InnerException);
            }
            catch (ResolvedAddressAuditDependencyException resolvedAddressAuditDependencyException)
            {
                return InternalServerError(resolvedAddressAuditDependencyException);
            }
            catch (ResolvedAddressAuditServiceException resolvedAddressAuditServiceException)
            {
                return InternalServerError(resolvedAddressAuditServiceException);
            }
        }
    }
}
