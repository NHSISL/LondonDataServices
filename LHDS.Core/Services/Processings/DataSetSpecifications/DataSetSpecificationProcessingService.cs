// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.DataSetSpecifications;

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

        public IQueryable<DataSetSpecification> RetrieveAllDataSetSpecifications() =>
            TryCatch(() => this.dataSetSpecificationService.RetrieveAllDataSetSpecifications());

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

        public ValueTask<DataSetSpecification> GetActiveDataSetSpecification(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateSupplierId(supplierId);

                DataSetSpecification result = this.dataSetSpecificationService.RetrieveAllDataSetSpecifications().Where(
                        specification =>
                                specification.DataSet.SupplierId == supplierId
                                && specification.DataSet.IsActive == true
                                && specification.IsActive == true).First();

                return await Task.FromResult(result);
            });
    }
}
