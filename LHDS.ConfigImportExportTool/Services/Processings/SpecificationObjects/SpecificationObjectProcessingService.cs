// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects
{
    internal partial class SpecificationObjectProcessingService : ISpecificationObjectProcessingService
    {
        private readonly ISpecificationObjectService specificationObjectService;
        private readonly ILoggingBroker loggingBroker;

        public SpecificationObjectProcessingService(
            ISpecificationObjectService specificationObjectService,
            ILoggingBroker loggingBroker)
        {
            this.specificationObjectService = specificationObjectService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SpecificationObject> ReadOrInsertSpecificationObjectAsync(
            SpecificationObject specificationObject) =>
            TryCatch(async () =>
            {
                ValidateSpecificationObjectProcessingOnRetrieveOrAdd(specificationObject);

                IQueryable<SpecificationObject> retrievedSpecificationObject =
                    await this.specificationObjectService.RetrieveAllSpecificationObjectsAsync();

                SpecificationObject maybeSpecificationObject =
                    retrievedSpecificationObject.FirstOrDefault(
                        item => item.SupplierObjectName == specificationObject.SupplierObjectName);

                if (maybeSpecificationObject == null)
                {
                    return await this.specificationObjectService.AddSpecificationObjectAsync(specificationObject);
                }

                return maybeSpecificationObject;
            });

        public ValueTask<IQueryable<SpecificationObject>> RetrieveAllSpecificationObjectsAsync() =>
            TryCatch(async () => await this.specificationObjectService.RetrieveAllSpecificationObjectsAsync());
    }
}
