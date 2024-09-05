// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.SpecificationObjects;

namespace LHDS.Core.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingService : ISpecificationObjectProcessingService
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

        public ValueTask<List<string>> RetrieveSpecificationObjectsByDataSetSpecificationId(
            Guid dataSetSpecificationId) =>
            TryCatch(async () =>
            {
                ValidateOnRetrieveSpecificationObjectsByDataSetSpecificationId(dataSetSpecificationId);

                return this.specificationObjectService
                    .RetrieveAllSpecificationObjects()
                    .Where(specificationObject => specificationObject.DataSetSpecificationId == dataSetSpecificationId)
                    .Select(specificationObject => specificationObject.SupplierObjectName)
                    .ToList();
            });
    }
}
