// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.KeyVaults;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SecureData;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService : ISecureDataService
    {
        private readonly ISecureDataBroker secureDataBroker;
        private readonly ILoggingBroker loggingBroker;

        public SecureDataService(
            ISecureDataBroker secureDataBroker,
            ILoggingBroker loggingBroker)
        {
            this.secureDataBroker = secureDataBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SecureData> AddOrModifySecureData(SecureData SecureData) =>
            throw new System.NotImplementedException();
            

    }
}