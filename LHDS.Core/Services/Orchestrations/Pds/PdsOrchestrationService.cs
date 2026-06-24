// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.TempLocations;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Foundations.PdsAudits;

namespace LHDS.Core.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationService : IPdsOrchestrationService
    {
        private readonly IPdsAuditService pdsAuditService;
        private readonly IDocumentService documentService;
        private readonly IMeshService meshService;
        private readonly BlobContainers blobContainers;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly PdsConfiguration pdsConfiguration;
        private readonly ITempLocationBroker tempLocationBroker;
        private readonly IFileBroker fileBroker;

        public PdsOrchestrationService(
            IPdsAuditService pdsAuditService,
            IDocumentService documentService,
            IMeshService meshService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            PdsConfiguration pdsConfiguration,
            ITempLocationBroker tempLocationBroker,
            IFileBroker fileBroker)
        {
            this.pdsAuditService = pdsAuditService;
            this.documentService = documentService;
            this.meshService = meshService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.pdsConfiguration = pdsConfiguration;
            this.tempLocationBroker = tempLocationBroker;
            this.fileBroker = fileBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync(
            CancellationToken cancellationToken = default) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await meshService.ValidateMailboxAccessAsync(cancellationToken);
            });

        public ValueTask<PdsAudit> PickupFileAndSendToMesh(
            Stream pdsStream,
            string fileName,
            CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            ValidateConfigurationSettings();
            ValidateBlobContainers();
            ValidatePdsArgs(pdsStream, fileName);

            DateTimeOffset timeStamp = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            Guid id = await this.identifierBroker.GetIdentifierAsync();
            Guid correlationId = await this.identifierBroker.GetIdentifierAsync();

            await this.meshService.SendMessageAsync(
                mexTo: this.pdsConfiguration.To,
                mexWorkflowId: this.pdsConfiguration.WorkflowId,
                content: pdsStream,
                mexSubject: string.Empty,
                mexLocalId: correlationId.ToString(),
                mexFileName: fileName,
                mexContentChecksum: string.Empty,
                contentType: "text/plain",
                contentEncoding: string.Empty,
                accept: "application/json",
                cancellationToken: cancellationToken);

            PdsAudit pdsAuditItem = await this.pdsAuditService
                .AddPdsAuditAsync(
                    new PdsAudit
                    {
                        Id = id,
                        CorrelationId = correlationId,
                        FileName = fileName,
                        Message = $"Sent message to mesh with id {correlationId}",
                        MessageId = correlationId.ToString(),
                        CreatedDate = timeStamp,
                        UpdatedDate = timeStamp,
                        CreatedBy = "System",
                        UpdatedBy = "System"
                    });

            return pdsAuditItem;
        });

        public ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage(
            CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            ValidateConfigurationSettings();
            ValidateBlobContainers();

            List<string> messageIds;
            var pdsAudits = new List<PdsAudit>();

            var exceptions = new List<Exception>();

            while ((messageIds = await this.meshService
                .RetrieveMessageIdsFromInboxAsync(cancellationToken)).Count > 0)
            {
                foreach (var id in messageIds)
                {
                    try
                    {
                        PdsAudit pdsAudit = await TryCatch(async () =>
                        {
                            string tempFilePath = this.tempLocationBroker.GetUniqueHomeFilePath();
                            MeshMessage message;

                            try
                            {
                                await using (FileStream outputStream = new FileStream(
                                    tempFilePath,
                                    FileMode.Create,
                                    FileAccess.Write,
                                    FileShare.None,
                                    bufferSize: 4096,
                                    useAsync: true))
                                {
                                    message = await this.meshService
                                        .RetrieveMessageByIdAsync(id, outputStream, cancellationToken);
                                }

                                if (message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.WorkflowId &&
                                    message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.ReturnWorkflowId)
                                {
                                    return null;
                                }

                                string filename = message.Headers["mex-filename"].FirstOrDefault();

                                string cleanedFileName =
                                    filename.StartsWith("RESP_") ? filename.Substring("RESP_".Length) : filename;

                                string fileName = $"{pdsConfiguration.OutputFolder}/{cleanedFileName}";

                                await using (FileStream inputStream = new FileStream(
                                    tempFilePath,
                                    FileMode.Open,
                                    FileAccess.Read,
                                    FileShare.Read,
                                    bufferSize: 4096,
                                    useAsync: true))
                                {
                                    await this.documentService.AddDocumentAsync(
                                        inputStream,
                                        fileName,
                                        container: blobContainers.Pds);
                                }

                                var correlationId = Guid.Parse(message.Headers["mex-localid"].FirstOrDefault());
                                DateTimeOffset currentDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                                var pdsAudit = new PdsAudit
                                {
                                    Id = await this.identifierBroker.GetIdentifierAsync(),
                                    CorrelationId = correlationId,
                                    FileName = fileName,
                                    Message = $"Received message from mesh with id {message.MessageId}",
                                    MessageId = message.MessageId,
                                    CreatedDate = currentDate,
                                    UpdatedDate = currentDate,
                                    CreatedBy = "System",
                                    UpdatedBy = "System"
                                };

                                await this.pdsAuditService.AddPdsAuditAsync(pdsAudit);

                                await this.meshService
                                    .AcknowledgeMessageByIdAsync(message.MessageId, cancellationToken);

                                return pdsAudit;
                            }
                            finally
                            {
                                await this.fileBroker.DeleteFileAsync(tempFilePath);
                            }
                        });

                        if (pdsAudit == null)
                        {
                            continue;
                        }

                        pdsAudits.Add(pdsAudit);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to retrieve message for {exceptions.Count} message IDs",
                        exceptions);
                }
            }

            return pdsAudits;
        });
    }
}
