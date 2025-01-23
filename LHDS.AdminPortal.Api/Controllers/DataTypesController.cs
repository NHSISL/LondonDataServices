// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using LHDS.Core.Services.Foundations.DataTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
    [ApiController]
    [Route("api/[controller]")]
    public class DataTypesController : RESTFulController
    {
        private readonly IDataTypeService dataTypeService;

        public DataTypesController(IDataTypeService dataTypeService) =>
            this.dataTypeService = dataTypeService;

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
        [HttpPost]
        public async ValueTask<ActionResult<DataType>> PostDataTypeAsync(DataType dataType)
        {
            try
            {
                DataType addedDataType =
                    await this.dataTypeService.AddDataTypeAsync(dataType);

                return Created(addedDataType);
            }
            catch (DataTypeValidationException dataTypeValidationException)
            {
                return BadRequest(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeValidationException)
                when (dataTypeValidationException.InnerException is InvalidDataTypeReferenceException)
            {
                return FailedDependency(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeDependencyValidationException)
               when (dataTypeDependencyValidationException.InnerException is AlreadyExistsDataTypeException)
            {
                return Conflict(dataTypeDependencyValidationException.InnerException);
            }
            catch (DataTypeDependencyException dataTypeDependencyException)
            {
                return InternalServerError(dataTypeDependencyException);
            }
            catch (DataTypeServiceException dataTypeServiceException)
            {
                return InternalServerError(dataTypeServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        public async ValueTask<ActionResult<IQueryable<DataType>>> Get()
        {
            try
            {
                IQueryable<DataType> retrievedDataTypes =
                    await this.dataTypeService.RetrieveAllDataTypesAsync();

                return Ok(retrievedDataTypes);
            }
            catch (DataTypeDependencyException dataTypeDependencyException)
            {
                return InternalServerError(dataTypeDependencyException);
            }
            catch (DataTypeServiceException dataTypeServiceException)
            {
                return InternalServerError(dataTypeServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet("{dataTypeId}")]
        public async ValueTask<ActionResult<DataType>> GetDataTypeByIdAsync(Guid dataTypeId)
        {
            try
            {
                DataType dataType = await this.dataTypeService.RetrieveDataTypeByIdAsync(dataTypeId);

                return Ok(dataType);
            }
            catch (DataTypeValidationException dataTypeValidationException)
                when (dataTypeValidationException.InnerException is NotFoundDataTypeException)
            {
                return NotFound(dataTypeValidationException.InnerException);
            }
            catch (DataTypeValidationException dataTypeValidationException)
            {
                return BadRequest(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyException dataTypeDependencyException)
            {
                return InternalServerError(dataTypeDependencyException);
            }
            catch (DataTypeServiceException dataTypeServiceException)
            {
                return InternalServerError(dataTypeServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
        [HttpPut]
        public async ValueTask<ActionResult<DataType>> PutDataTypeAsync(DataType dataType)
        {
            try
            {
                DataType modifiedDataType =
                    await this.dataTypeService.ModifyDataTypeAsync(dataType);

                return Ok(modifiedDataType);
            }
            catch (DataTypeValidationException dataTypeValidationException)
                when (dataTypeValidationException.InnerException is NotFoundDataTypeException)
            {
                return NotFound(dataTypeValidationException.InnerException);
            }
            catch (DataTypeValidationException dataTypeValidationException)
            {
                return BadRequest(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeValidationException)
                when (dataTypeValidationException.InnerException is InvalidDataTypeReferenceException)
            {
                return FailedDependency(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeDependencyValidationException)
               when (dataTypeDependencyValidationException.InnerException is AlreadyExistsDataTypeException)
            {
                return Conflict(dataTypeDependencyValidationException.InnerException);
            }
            catch (DataTypeDependencyException dataTypeDependencyException)
            {
                return InternalServerError(dataTypeDependencyException);
            }
            catch (DataTypeServiceException dataTypeServiceException)
            {
                return InternalServerError(dataTypeServiceException);
            }
        }

        [HttpDelete("{dataTypeId}")]
        public async ValueTask<ActionResult<DataType>> DeleteDataTypeByIdAsync(Guid dataTypeId)
        {
            try
            {
                DataType deletedDataType =
                    await this.dataTypeService.RemoveDataTypeByIdAsync(dataTypeId);

                return Ok(deletedDataType);
            }
            catch (DataTypeValidationException dataTypeValidationException)
                when (dataTypeValidationException.InnerException is NotFoundDataTypeException)
            {
                return NotFound(dataTypeValidationException.InnerException);
            }
            catch (DataTypeValidationException dataTypeValidationException)
            {
                return BadRequest(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeDependencyValidationException)
                when (dataTypeDependencyValidationException.InnerException is LockedDataTypeException)
            {
                return Locked(dataTypeDependencyValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeDependencyValidationException)
            {
                return BadRequest(dataTypeDependencyValidationException);
            }
            catch (DataTypeDependencyException dataTypeDependencyException)
            {
                return InternalServerError(dataTypeDependencyException);
            }
            catch (DataTypeServiceException dataTypeServiceException)
            {
                return InternalServerError(dataTypeServiceException);
            }
        }
    }
}