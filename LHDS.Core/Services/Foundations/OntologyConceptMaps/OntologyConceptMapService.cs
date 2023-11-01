using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;

namespace LHDS.Core.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapService : IOntologyConceptMapService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public OntologyConceptMapService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<OntologyConceptMap> AddOntologyConceptMapAsync(OntologyConceptMap ontologyConceptMap) =>
            TryCatch(async () =>
            {
                ValidateOntologyConceptMapOnAdd(ontologyConceptMap);

                return await this.storageBroker.InsertOntologyConceptMapAsync(ontologyConceptMap);
            });

        public IQueryable<OntologyConceptMap> RetrieveAllOntologyConceptMaps() =>
            TryCatch(() => this.storageBroker.SelectAllOntologyConceptMaps());

        public ValueTask<OntologyConceptMap> RetrieveOntologyConceptMapByIdAsync(Guid ontologyConceptMapId) =>
            TryCatch(async () =>
            {
                ValidateOntologyConceptMapId(ontologyConceptMapId);

                OntologyConceptMap maybeOntologyConceptMap = await this.storageBroker
                    .SelectOntologyConceptMapByIdAsync(ontologyConceptMapId);

                ValidateStorageOntologyConceptMap(maybeOntologyConceptMap, ontologyConceptMapId);

                return maybeOntologyConceptMap;
            });

        public ValueTask<OntologyConceptMap> ModifyOntologyConceptMapAsync(OntologyConceptMap ontologyConceptMap) =>
            TryCatch(async () =>
            {
                ValidateOntologyConceptMapOnModify(ontologyConceptMap);

                OntologyConceptMap maybeOntologyConceptMap =
                    await this.storageBroker.SelectOntologyConceptMapByIdAsync(ontologyConceptMap.Id);

                ValidateStorageOntologyConceptMap(maybeOntologyConceptMap, ontologyConceptMap.Id);
                ValidateAgainstStorageOntologyConceptMapOnModify(inputOntologyConceptMap: ontologyConceptMap, storageOntologyConceptMap: maybeOntologyConceptMap);

                return await this.storageBroker.UpdateOntologyConceptMapAsync(ontologyConceptMap);
            });

        public ValueTask<OntologyConceptMap> RemoveOntologyConceptMapByIdAsync(Guid ontologyConceptMapId) =>
            throw new NotImplementedException();
    }
}