// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.DateTimes;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectService : ISpecificationObjectService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public SpecificationObjectService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SpecificationObject> AddSpecificationObjectAsync(SpecificationObject specificationObject) =>
            TryCatch(async () =>
            {
                await ValidateSpecificationObjectOnAddAsync(specificationObject);

                return await storageBroker.InsertSpecificationObjectAsync(specificationObject);
            });

        public ValueTask<IQueryable<SpecificationObject>> RetrieveAllSpecificationObjectsAsync() =>
            TryCatch(async () => await storageBroker.SelectAllSpecificationObjectsAsync());

        public ValueTask<SpecificationObject> RetrieveSpecificationObjectByIdAsync(Guid specificationObjectId) =>
            TryCatch(async () =>
            {
                ValidateSpecificationObjectId(specificationObjectId);

                SpecificationObject maybeSpecificationObject = await storageBroker
                    .SelectSpecificationObjectByIdAsync(specificationObjectId);

                ValidateStorageSpecificationObject(maybeSpecificationObject, specificationObjectId);

                return maybeSpecificationObject;
            });

        public ValueTask<SpecificationObject> ModifySpecificationObjectAsync(SpecificationObject specificationObject) =>
            TryCatch(async () =>
            {
                await ValidateSpecificationObjectOnModifyAsync(specificationObject);

                SpecificationObject maybeSpecificationObject =
                    await storageBroker.SelectSpecificationObjectByIdAsync(specificationObject.Id);

                ValidateStorageSpecificationObject(maybeSpecificationObject, specificationObject.Id);

                ValidateAgainstStorageSpecificationObjectOnModify(
                    inputSpecificationObject: specificationObject,
                    storageSpecificationObject: maybeSpecificationObject);

                return await storageBroker.UpdateSpecificationObjectAsync(specificationObject);
            });

        public ValueTask<SpecificationObject> RemoveSpecificationObjectByIdAsync(Guid specificationObjectId) =>
            TryCatch(async () =>
            {
                ValidateSpecificationObjectId(specificationObjectId);

                SpecificationObject maybeSpecificationObject = await storageBroker
                    .SelectSpecificationObjectByIdAsync(specificationObjectId);

                ValidateStorageSpecificationObject(maybeSpecificationObject, specificationObjectId);

                return await storageBroker.DeleteSpecificationObjectAsync(maybeSpecificationObject);
            });
    }
}