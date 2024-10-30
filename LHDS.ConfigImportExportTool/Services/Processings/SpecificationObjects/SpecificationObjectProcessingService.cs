// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

        public async ValueTask<SpecificationObject> ReadOrInsertSpecificationObjectAsync(
            SpecificationObject specificationObject) =>
                throw new NotImplementedException();

        public ValueTask<IQueryable<SpecificationObject>> RetrieveAllSpecificationObjectsAsync() =>
            TryCatch(async () => await this.specificationObjectService.RetrieveAllSpecificationObjectsAsync());
    }
}
