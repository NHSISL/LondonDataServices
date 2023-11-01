using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;

namespace LHDS.Core.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemService : IOntologyCodeSystemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public OntologyCodeSystemService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<OntologyCodeSystem> AddOntologyCodeSystemAsync(OntologyCodeSystem ontologyCodeSystem) =>
            TryCatch(async () =>
            {
                ValidateOntologyCodeSystemOnAdd(ontologyCodeSystem);

                return await this.storageBroker.InsertOntologyCodeSystemAsync(ontologyCodeSystem);
            });

        public IQueryable<OntologyCodeSystem> RetrieveAllOntologyCodeSystems() =>
            TryCatch(() => this.storageBroker.SelectAllOntologyCodeSystems());

        public ValueTask<OntologyCodeSystem> RetrieveOntologyCodeSystemByIdAsync(Guid ontologyCodeSystemId) =>
            TryCatch(async () =>
            {
                ValidateOntologyCodeSystemId(ontologyCodeSystemId);

                OntologyCodeSystem maybeOntologyCodeSystem = await this.storageBroker
                    .SelectOntologyCodeSystemByIdAsync(ontologyCodeSystemId);

                ValidateStorageOntologyCodeSystem(maybeOntologyCodeSystem, ontologyCodeSystemId);

                return maybeOntologyCodeSystem;
            });

        public ValueTask<OntologyCodeSystem> ModifyOntologyCodeSystemAsync(OntologyCodeSystem ontologyCodeSystem) =>
            TryCatch(async () =>
            {
                ValidateOntologyCodeSystemOnModify(ontologyCodeSystem);

                return await this.storageBroker.UpdateOntologyCodeSystemAsync(ontologyCodeSystem);
            });
    }
}