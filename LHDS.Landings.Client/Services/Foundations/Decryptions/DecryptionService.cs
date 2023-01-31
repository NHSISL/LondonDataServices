// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Decryptions;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages;

namespace LHDS.Landings.Client.Services.Foundations.Decryptions
{
    public partial class DecryptionService : IDecryptionService
    {

        private readonly IDecryptionBroker decryptionBroker;
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DecryptionService(
            IDecryptionBroker decryptionBroker,
        IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.decryptionBroker = decryptionBroker;
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async Task<byte[]> DecryptAsync(byte[] data) =>
            throw new System.NotImplementedException();
    }
}