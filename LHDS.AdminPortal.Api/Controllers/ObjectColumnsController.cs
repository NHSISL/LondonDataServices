// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.Core.Services.Foundations.ObjectColumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectColumnsController : RESTFulController
    {
        private readonly IObjectColumnService objectColumnService;

        public ObjectColumnsController(IObjectColumnService objectColumnService) =>
            this.objectColumnService = objectColumnService;

        [HttpPost]
        public async ValueTask<ActionResult<ObjectColumn>> PostObjectColumnAsync(ObjectColumn objectColumn)
        {
            try
            {
                ObjectColumn addedObjectColumn =
                    await this.objectColumnService.AddObjectColumnAsync(objectColumn);

                return Created(addedObjectColumn);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                return BadRequest(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnValidationException)
                when (objectColumnValidationException.InnerException is InvalidObjectColumnReferenceException)
            {
                return FailedDependency(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
               when (objectColumnDependencyValidationException.InnerException is AlreadyExistsObjectColumnException)
            {
                return Conflict(objectColumnDependencyValidationException.InnerException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                return InternalServerError(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                return InternalServerError(objectColumnServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.ObjectColumns, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IQueryable<ObjectColumn>>> Get()
        {
            try
            {
                IQueryable<ObjectColumn> retrievedObjectColumns =
                    await this.objectColumnService.RetrieveAllObjectColumnsAsync();

                return Ok(retrievedObjectColumns);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                return InternalServerError(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                return InternalServerError(objectColumnServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
        [HttpGet("{objectColumnId}")]
        public async ValueTask<ActionResult<ObjectColumn>> GetObjectColumnByIdAsync(Guid objectColumnId)
        {
            try
            {
                ObjectColumn objectColumn = await this.objectColumnService.RetrieveObjectColumnByIdAsync(objectColumnId);

                return Ok(objectColumn);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
                when (objectColumnValidationException.InnerException is NotFoundObjectColumnException)
            {
                return NotFound(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                return BadRequest(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                return InternalServerError(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                return InternalServerError(objectColumnServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
        [HttpPut]
        public async ValueTask<ActionResult<ObjectColumn>> PutObjectColumnAsync(ObjectColumn objectColumn)
        {
            try
            {
                ObjectColumn modifiedObjectColumn =
                    await this.objectColumnService.ModifyObjectColumnAsync(objectColumn);

                return Ok(modifiedObjectColumn);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
                when (objectColumnValidationException.InnerException is NotFoundObjectColumnException)
            {
                return NotFound(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                return BadRequest(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnValidationException)
                when (objectColumnValidationException.InnerException is InvalidObjectColumnReferenceException)
            {
                return FailedDependency(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
               when (objectColumnDependencyValidationException.InnerException is AlreadyExistsObjectColumnException)
            {
                return Conflict(objectColumnDependencyValidationException.InnerException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                return InternalServerError(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                return InternalServerError(objectColumnServiceException);
            }
        }

        [HttpDelete("{objectColumnId}")]
        public async ValueTask<ActionResult<ObjectColumn>> DeleteObjectColumnByIdAsync(Guid objectColumnId)
        {
            try
            {
                ObjectColumn deletedObjectColumn =
                    await this.objectColumnService.RemoveObjectColumnByIdAsync(objectColumnId);

                return Ok(deletedObjectColumn);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
                when (objectColumnValidationException.InnerException is NotFoundObjectColumnException)
            {
                return NotFound(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                return BadRequest(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
                when (objectColumnDependencyValidationException.InnerException is LockedObjectColumnException)
            {
                return Locked(objectColumnDependencyValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
            {
                return BadRequest(objectColumnDependencyValidationException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                return InternalServerError(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                return InternalServerError(objectColumnServiceException);
            }
        }
    }
}