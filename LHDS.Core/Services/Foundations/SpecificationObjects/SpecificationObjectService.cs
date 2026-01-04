// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.SpecificationObjects;

namespace LHDS.Core.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectService : ISpecificationObjectService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public SpecificationObjectService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityAuditBroker securityAuditBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SpecificationObject> AddSpecificationObjectAsync(SpecificationObject specificationObject) =>
            TryCatch(async () =>
            {
                SpecificationObject specificationObjectWithAddAuditApplied = 
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(specificationObject);

                await ValidateSpecificationObjectOnAddAsync(specificationObjectWithAddAuditApplied);

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
                SpecificationObject specificationObjectWithModifyAuditApplied = 
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(specificationObject);

                await ValidateSpecificationObjectOnModifyAsync(specificationObjectWithModifyAuditApplied);

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

                SpecificationObject maybeSpecificationObject = 
                    await this.storageBroker.SelectSpecificationObjectByIdAsync(specificationObjectId);

                ValidateStorageSpecificationObject(maybeSpecificationObject, specificationObjectId);

                SpecificationObject specificationObjectWithDeleteAuditApplied = 
                    await this.securityAuditBroker.ApplyRemoveAuditValuesAsync(maybeSpecificationObject);

                SpecificationObject updatedSpecificationObject =
                    await this.storageBroker.UpdateSpecificationObjectAsync(specificationObjectWithDeleteAuditApplied);

                await ValidateAgainstStorageSpecificationObjectOnDeleteAsync(
                    updatedSpecificationObject,
                    specificationObjectWithDeleteAuditApplied);

                return await this.storageBroker.DeleteSpecificationObjectAsync(updatedSpecificationObject);
            });
    }
}