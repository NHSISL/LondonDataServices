// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns
{
    internal partial class ObjectColumnProcessingService : IObjectColumnProcessingService
    {
        private readonly IObjectColumnService ObjectColumnService;
        private readonly ILoggingBroker loggingBroker;

        public ObjectColumnProcessingService(
            IObjectColumnService ObjectColumnService,
            ILoggingBroker loggingBroker)
        {
            this.ObjectColumnService = ObjectColumnService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ObjectColumn> ReadOrInsertObjectColumnAsync(
            ObjectColumn ObjectColumn) =>
            TryCatch(async () =>
            {
                ValidateObjectColumnProcessingOnRetrieveOrAdd(ObjectColumn);

                IQueryable<ObjectColumn> retrievedObjectColumn =
                    await this.ObjectColumnService.RetrieveAllObjectColumnsAsync();

                ObjectColumn maybeObjectColumn =
                    retrievedObjectColumn.FirstOrDefault(
                        item => item.SupplierColumnName == ObjectColumn.SupplierColumnName
                        && item.SpecificationObjectId == ObjectColumn.SpecificationObjectId);

                if (maybeObjectColumn == null)
                {
                    return await this.ObjectColumnService.AddObjectColumnAsync(ObjectColumn);
                }

                return maybeObjectColumn;
            });

        public ValueTask<IQueryable<ObjectColumn>> RetrieveAllObjectColumnsAsync() =>
            TryCatch(async () => await this.ObjectColumnService.RetrieveAllObjectColumnsAsync());
    }
}
