// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Services.Foundations.ObjectColumns;

namespace LHDS.Core.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingService : IObjectColumnProcessingService
    {
        private readonly IObjectColumnService objectColumnService;
        private readonly ILoggingBroker loggingBroker;

        public ObjectColumnProcessingService(
            IObjectColumnService objectColumnService,
            ILoggingBroker loggingBroker)
        {
            this.objectColumnService = objectColumnService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ObjectColumn> AddObjectColumnAsync(ObjectColumn objectColumn) =>
            TryCatch(async () =>
            {
                ValidateObjectColumn(objectColumn);

                return await this.objectColumnService.AddObjectColumnAsync(objectColumn);
            });

        public IQueryable<ObjectColumn> RetrieveAllObjectColumns() =>
            TryCatch(() => this.objectColumnService.RetrieveAllObjectColumns());

        public ValueTask<ObjectColumn> RetrieveObjectColumnByIdAsync(Guid objectColumnId) =>
            TryCatch(async () =>
            {
                ValidateObjectColumnId(objectColumnId);

                return await this.objectColumnService.RetrieveObjectColumnByIdAsync(objectColumnId);
            });

        public ValueTask<ObjectColumn> RetrieveOrAddObjectColumnAsync(ObjectColumn objectColumn) =>
            throw new NotImplementedException();
    }
}
