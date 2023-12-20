// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;

namespace LHDS.Core.Services.Orchestrations.TerminologyDetails
{
    internal partial class TerminologyDetailOrchestrationService : ITerminologyDetailOrchestrationService
    {
        private readonly ITerminologyArtifactProcessingService terminologyArtifactProcessingService;
        private readonly IOntologyProcessingService ontologyProcessingService;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TerminologyDetailOrchestrationService(
            ITerminologyArtifactProcessingService terminologyArtifactProcessingService,
            IOntologyProcessingService ontologyProcessingService,
            IDocumentProcessingService documentProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.terminologyArtifactProcessingService = terminologyArtifactProcessingService;
            this.ontologyProcessingService = ontologyProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask RetrieveArtifactDetailsAsync() =>
            TryCatch(async () =>
            {
                TerminologyArtifact artifact = null;

                while ((artifact =
                    await this.terminologyArtifactProcessingService.GetNonDownloadedArtifactAsync()) != null)
                {
                    string relativeUrl = artifact.FullUrl;

                    string artifactDetail =
                        await this.ontologyProcessingService.RetrieveArtifactDetailsAsync(relativeUrl);

                    byte[] artifactDetailData = Encoding.UTF8.GetBytes(artifactDetail);

                    Document artifactDetailDocument = new Document
                    {
                        FileName = $"{artifact.ResourceType}/{artifact.Name}.json",
                        DocumentData = artifactDetailData
                    };

                    await this.documentProcessingService.AddDocumentAsync(artifactDetailDocument, "terminology");
                    artifact.IsDownloaded = true;
                    TerminologyArtifact modifiedArtifact 
                        = await this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(artifact);
                }
            });
    }
}