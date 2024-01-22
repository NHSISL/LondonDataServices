// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Decryptions;
using LHDS.Core.Brokers.Loggings;

namespace LHDS.Core.Services.Foundations.Cryptographies
{
    public partial class CryptographyService : ICryptographyService
    {

        private readonly ICryptographyBroker cryptographyBroker;
        private readonly ILoggingBroker loggingBroker;

        public CryptographyService(
            ICryptographyBroker cryptographyBroker,
            ILoggingBroker loggingBroker)
        {
            this.cryptographyBroker = cryptographyBroker;
            this.loggingBroker = loggingBroker;
        }

        public Task<byte[]> EncryptAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateData(data);

                return await this.cryptographyBroker.EncryptAsync(data);
            });

        public Task<byte[]> DecryptAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateData(data);

                return await this.cryptographyBroker.DecryptAsync(data);
            });
    }
}