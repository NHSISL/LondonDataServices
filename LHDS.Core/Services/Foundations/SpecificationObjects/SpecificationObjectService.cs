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
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public SpecificationObjectService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SpecificationObject> AddSpecificationObjectAsync(SpecificationObject specificationObject) =>
            TryCatch(async () =>
            {
                SpecificationObject specificationObjectWithAddAuditApplied = 
                    await ApplyAddAuditAsync(specificationObject);

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
                    await ApplyModifyAuditAsync(specificationObject);

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

                SpecificationObject maybeSpecificationObject = await storageBroker
                    .SelectSpecificationObjectByIdAsync(specificationObjectId);

                ValidateStorageSpecificationObject(maybeSpecificationObject, specificationObjectId);

                return await storageBroker.DeleteSpecificationObjectAsync(maybeSpecificationObject);
            });

        virtual internal async ValueTask<SpecificationObject> 
            ApplyAddAuditAsync(SpecificationObject specificationObject)
        {
            ValidateSpecificationObjectIsNotNull(specificationObject);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            specificationObject.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            specificationObject.CreatedDate = auditDateTimeOffset;
            specificationObject.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            specificationObject.UpdatedDate = auditDateTimeOffset;

            return specificationObject;
        }

        virtual internal async ValueTask<SpecificationObject> 
            ApplyModifyAuditAsync(SpecificationObject specificationObject)
        {
            ValidateSpecificationObjectIsNotNull(specificationObject);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            specificationObject.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            specificationObject.UpdatedDate = auditDateTimeOffset;

            return specificationObject;
        }
    }
}