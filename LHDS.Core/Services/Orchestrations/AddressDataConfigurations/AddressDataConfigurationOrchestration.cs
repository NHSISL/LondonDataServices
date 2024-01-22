// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.Files;

namespace LHDS.Core.Services.Orchestrations.AddressDataConfigurations
{
    internal class AddressDataConfigurationOrchestration : IAddressDataConfigurationOrchestration
    {
        private readonly IDownloadService downloadService;
        private readonly IFileService fileService;
        private readonly ILoggingBroker loggingBroker;

        public AddressDataConfigurationOrchestration(
            IDownloadService downloadService,
            IFileService fileService,
            ILoggingBroker loggingBroker)
        {
            this.downloadService = downloadService;
            this.fileService = fileService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> LoadConfiguration() =>
            throw new NotImplementedException();
    }
}
