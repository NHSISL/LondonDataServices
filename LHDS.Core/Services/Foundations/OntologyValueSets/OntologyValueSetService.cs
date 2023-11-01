using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.OntologyValueSets;

namespace LHDS.Core.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetService : IOntologyValueSetService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public OntologyValueSetService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<OntologyValueSet> AddOntologyValueSetAsync(OntologyValueSet ontologyValueSet) =>
            TryCatch(async () =>
            {
                ValidateOntologyValueSetOnAdd(ontologyValueSet);

                return await this.storageBroker.InsertOntologyValueSetAsync(ontologyValueSet);
            });

        public IQueryable<OntologyValueSet> RetrieveAllOntologyValueSets() =>
            TryCatch(() => this.storageBroker.SelectAllOntologyValueSets());

        public ValueTask<OntologyValueSet> RetrieveOntologyValueSetByIdAsync(Guid ontologyValueSetId) =>
            TryCatch(async () =>
            {
                ValidateOntologyValueSetId(ontologyValueSetId);

                OntologyValueSet maybeOntologyValueSet = await this.storageBroker
                    .SelectOntologyValueSetByIdAsync(ontologyValueSetId);

                ValidateStorageOntologyValueSet(maybeOntologyValueSet, ontologyValueSetId);

                return maybeOntologyValueSet;
            });

        public ValueTask<OntologyValueSet> ModifyOntologyValueSetAsync(OntologyValueSet ontologyValueSet) =>
            TryCatch(async () =>
            {
                ValidateOntologyValueSetOnModify(ontologyValueSet);

                OntologyValueSet maybeOntologyValueSet =
                    await this.storageBroker.SelectOntologyValueSetByIdAsync(ontologyValueSet.Id);

                ValidateStorageOntologyValueSet(maybeOntologyValueSet, ontologyValueSet.Id);
                ValidateAgainstStorageOntologyValueSetOnModify(inputOntologyValueSet: ontologyValueSet, storageOntologyValueSet: maybeOntologyValueSet);

                return await this.storageBroker.UpdateOntologyValueSetAsync(ontologyValueSet);
            });

        public ValueTask<OntologyValueSet> RemoveOntologyValueSetByIdAsync(Guid ontologyValueSetId) =>
            TryCatch(async () =>
            {
                ValidateOntologyValueSetId(ontologyValueSetId);

                OntologyValueSet maybeOntologyValueSet = await this.storageBroker
                    .SelectOntologyValueSetByIdAsync(ontologyValueSetId);

                ValidateStorageOntologyValueSet(maybeOntologyValueSet, ontologyValueSetId);

                return await this.storageBroker.DeleteOntologyValueSetAsync(maybeOntologyValueSet);
            });
    }
}