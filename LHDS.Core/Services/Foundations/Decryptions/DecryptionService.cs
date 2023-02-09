// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Decryptions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;

namespace LHDS.Core.Services.Foundations.Decryptions
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

        public Task<byte[]> DecryptAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateDecryptionOnDecrypt(data);

                return await this.decryptionBroker.DecryptAsync(data);
            });
    }
}