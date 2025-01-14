// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingService : IDataSetSpecificationProcessingService
    {
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly ILoggingBroker loggingBroker;

        public DataSetSpecificationProcessingService(
            IDataSetSpecificationService dataSetSpecificationService,
            ILoggingBroker loggingBroker)
        {
            this.dataSetSpecificationService = dataSetSpecificationService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<DataSetSpecification> AddDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
                TryCatch(async () =>
                {
                    ValidateDataSetSpecification(dataSetSpecification);

                    return await this.dataSetSpecificationService.AddDataSetSpecificationAsync(dataSetSpecification);
                });

        public ValueTask<IQueryable<DataSetSpecification>> RetrieveAllDataSetSpecificationsAsync() =>
            TryCatch(async() => await this.dataSetSpecificationService.RetrieveAllDataSetSpecificationsAsync());

        public ValueTask<DataSetSpecification> RetrieveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            TryCatch(async () =>
            {
                ValidateDataSetSpecificationId(dataSetSpecificationId);

                return await this.dataSetSpecificationService
                    .RetrieveDataSetSpecificationByIdAsync(dataSetSpecificationId);
            });

        public ValueTask<DataSetSpecification> RetrieveOrAddDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
                TryCatch(async () =>
                {
                    ValidateDataSetSpecification(dataSetSpecification);
                    ValidateDataSetSpecificationId(dataSetSpecification.Id);

                    return await this.dataSetSpecificationService.RetrieveDataSetSpecificationByIdAsync(dataSetSpecification.Id) ??
                        await this.dataSetSpecificationService.AddDataSetSpecificationAsync(dataSetSpecification);
                });

        public ValueTask<DataSetSpecification> ModifyOrAddDataSetSpecificationAsync(DataSetSpecification dataSetSpecification) =>
            TryCatch(async () =>
            {
                ValidateDataSetSpecification(dataSetSpecification);
                ValidateDataSetSpecificationId(dataSetSpecification.Id);

                var maybeDataSetSpecification = await this.dataSetSpecificationService
                    .RetrieveDataSetSpecificationByIdAsync(dataSetSpecification.Id);

                if (maybeDataSetSpecification != null)
                {
                    return await this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(dataSetSpecification);
                }
                else
                {
                    return await this.dataSetSpecificationService.AddDataSetSpecificationAsync(dataSetSpecification);
                }
            });

        public ValueTask<DataSetSpecification> ModifyDataSetSpecificationAsync(DataSetSpecification dataSetSpecification) =>
            TryCatch(async () =>
            {
                ValidateDataSetSpecification(dataSetSpecification);

                return await this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(dataSetSpecification);
            });

        public ValueTask<DataSetSpecification> RemoveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            TryCatch(async () =>
            {
                ValidateDataSetSpecificationId(dataSetSpecificationId);

                return await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(dataSetSpecificationId);
            });

        public ValueTask<DataSetSpecification?> GetActiveDataSetSpecification(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateSupplierId(supplierId);

                IQueryable<DataSetSpecification> allDataSetSpecifications =
                    await this.dataSetSpecificationService.RetrieveAllDataSetSpecificationsAsync();

                List<DataSetSpecification> result = allDataSetSpecifications
                    .Include(specification => specification.DataSet)
                    .Where(specification => specification.DataSet.SupplierId == supplierId
                        && specification.DataSet.IsActive == true
                        && specification.IsActive == true).ToList();

                ValidateDataSetSpecificationCount(count: result.Count());

                return await Task.FromResult(result.FirstOrDefault());
            });
    }
}
