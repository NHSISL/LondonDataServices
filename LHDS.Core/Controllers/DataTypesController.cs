using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using LHDS.Core.Services.Foundations.DataTypes;

namespace LHDS.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataTypesController : RESTFulController
    {
        private readonly IDataTypeService dataTypeService;

        public DataTypesController(IDataTypeService dataTypeService) =>
            this.dataTypeService = dataTypeService;

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
        public ActionResult<IQueryable<DataType>> GetAllDataTypes()
        {
            try
            {
                IQueryable<DataType> retrievedDataTypes =
                    this.dataTypeService.RetrieveAllDataTypes();

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
    }
}