// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Foundations.Documents;
using Microsoft.Extensions.Configuration;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DocumentService : IDocumentService
    {
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IConfiguration configuration;

        public DocumentService(IBlobStorageBroker blobStorageBroker, ILoggingBroker loggingBroker, IConfiguration configuration)
        {
            this.blobStorageBroker = blobStorageBroker;
            this.loggingBroker = loggingBroker;
            this.configuration = configuration;
        }

        public ValueTask AddDocumentAsync(Document document) =>
            TryCatch(async () =>
            {
                ValidateDocumentIsNotNull(document);

                await this.blobStorageBroker.InsertFileAsync(
                   fileName: document.FileName,
                   stream: document.DocumentStream,
                   container: this.configuration.GetValue<string>("blobContainerName"));
            });
    }
}
