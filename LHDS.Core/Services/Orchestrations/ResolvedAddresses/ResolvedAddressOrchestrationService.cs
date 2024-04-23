// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    internal partial class ResolvedAddressOrchestrationService : IResolvedAddressOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly IResolvedAddressService resolvedAddressService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public ResolvedAddressOrchestrationService(
            IDocumentService documentService,
            IResolvedAddressService resolvedAddressService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.documentService = documentService;
            this.resolvedAddressService = resolvedAddressService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Guid> UploadResolvedAddressesAsync() =>
            throw new NotImplementedException();
        
        public ValueTask AddDocumentAsync(byte[] data, string filename, string container) =>
            throw new NotImplementedException();

        public ValueTask RemoveDocumentByFileNameAsync(string filename, string container) =>
            throw new NotImplementedException();
    }
}
