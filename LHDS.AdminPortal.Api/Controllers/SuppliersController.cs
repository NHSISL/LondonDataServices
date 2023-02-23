using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Suppliers;
using LHDS.AdminPortal.Api.Models.Suppliers.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.Suppliers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : RESTFulController
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService) =>
            this.supplierService = supplierService;

        [HttpPost]
        public async ValueTask<ActionResult<Supplier>> PostSupplierAsync(Supplier supplier)
        {
            try
            {
                Supplier addedSupplier =
                    await this.supplierService.AddSupplierAsync(supplier);

                return Created(addedSupplier);
            }
            catch (SupplierValidationException supplierValidationException)
            {
                return BadRequest(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierValidationException)
                when (supplierValidationException.InnerException is InvalidSupplierReferenceException)
            {
                return FailedDependency(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierDependencyValidationException)
               when (supplierDependencyValidationException.InnerException is AlreadyExistsSupplierException)
            {
                return Conflict(supplierDependencyValidationException.InnerException);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Supplier>> GetAllSuppliers()
        {
            try
            {
                IQueryable<Supplier> retrievedSuppliers =
                    this.supplierService.RetrieveAllSuppliers();

                return Ok(retrievedSuppliers);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }

        [HttpGet("{supplierId}")]
        public async ValueTask<ActionResult<Supplier>> GetSupplierByIdAsync(Guid supplierId)
        {
            try
            {
                Supplier supplier = await this.supplierService.RetrieveSupplierByIdAsync(supplierId);

                return Ok(supplier);
            }
            catch (SupplierValidationException supplierValidationException)
                when (supplierValidationException.InnerException is NotFoundSupplierException)
            {
                return NotFound(supplierValidationException.InnerException);
            }
            catch (SupplierValidationException supplierValidationException)
            {
                return BadRequest(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }
    }
}