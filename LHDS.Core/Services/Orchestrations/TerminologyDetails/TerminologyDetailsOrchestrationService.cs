// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
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
        private readonly BlobContainers blobContainers;

        public TerminologyDetailOrchestrationService(
            ITerminologyArtifactProcessingService terminologyArtifactProcessingService,
            IOntologyProcessingService ontologyProcessingService,
            IDocumentProcessingService documentProcessingService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.terminologyArtifactProcessingService = terminologyArtifactProcessingService;
            this.ontologyProcessingService = ontologyProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask RetrieveArtifactDetailsAsync() =>
            TryCatch(async () =>
            {
                TerminologyArtifact artifact = null;
                var exceptions = new List<Exception>();

                while ((artifact =
                    await this.terminologyArtifactProcessingService.GetNonDownloadedArtifactAsync()) != null)
                {
                    try
                    {
                        await TryCatch(async () =>
                        {
                            string relativeUrl = artifact.FullUrl;

                            string artifactDetail = await this.ontologyProcessingService
                                .RetrieveArtifactDetailsAsync(relativeUrl);

                            byte[] artifactDetailData = Encoding.UTF8.GetBytes(artifactDetail);
                            string fileName = $"Core/{artifact.ResourceType}/{artifact.Name}.json";

                            using (Stream input = new MemoryStream(artifactDetailData))
                            {
                                await this.documentProcessingService.AddDocumentAsync(
                                    input,
                                    fileName,
                                    container: blobContainers.Terminology);
                            }

                            artifact.IsDownloaded = true;
                            artifact.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();

                            await this.terminologyArtifactProcessingService
                                .ModifyOrAddTerminologyArtifactAsync(artifact);
                        });
                    }
                    catch (Exception ex)
                    {
                        artifact.IsDownloaded = false;
                        artifact.IsError = true;

                        artifact.ErrorMessage = ex?.InnerException?.InnerException?.Message
                            ?? ex?.InnerException?.Message
                            ?? ex?.Message;

                        exceptions.Add(ex);
                        await this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(artifact);
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to retrieve terminology artifact details for {exceptions.Count} message IDs",
                        exceptions);
                }
            });

        public ValueTask RetrieveUserArtifactDetailsAsync() =>
            TryCatch(async () =>
            {
                TerminologyArtifact artifact = null;
                var exceptions = new List<Exception>();

                while ((artifact =
                    await this.terminologyArtifactProcessingService.GetNonDownloadedUserArtifactAsync()) != null)
                {
                    try
                    {
                        await TryCatch(async () =>
                        {
                            string relativeUrl = artifact.FullUrl;

                            string artifactDetail = await this.ontologyProcessingService
                                .RetrieveArtifactDetailsAsync(relativeUrl);

                            byte[] artifactDetailData = Encoding.UTF8.GetBytes(artifactDetail);
                            string fileName = $"Users/{artifact.ResourceType}/{artifact.Name}.json";

                            using (Stream input = new MemoryStream(artifactDetailData))
                            {
                                await this.documentProcessingService.AddDocumentAsync(
                                    input,
                                    fileName,
                                    container: blobContainers.Terminology);
                            }

                            artifact.IsDownloadedForUser = true;
                            artifact.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();

                            await this.terminologyArtifactProcessingService
                                .ModifyOrAddTerminologyArtifactAsync(artifact);
                        });
                    }
                    catch (Exception ex)
                    {
                        artifact.IsDownloadedForUser = false;
                        artifact.IsError = true;

                        artifact.ErrorMessage = ex?.InnerException?.InnerException?.Message
                            ?? ex?.InnerException?.Message
                            ?? ex?.Message;

                        exceptions.Add(ex);
                        await this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(artifact);
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to retrieve terminology artifact details for {exceptions.Count} message IDs",
                        exceptions);
                }
            });
    }
}