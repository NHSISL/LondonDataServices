// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
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

        public PdsOrchestrationService(
            IPdsAuditService pdsAuditService,
            IDocumentService documentService,
            IMeshService meshService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            PdsConfiguration pdsConfiguration
            )
        {
            this.pdsAuditService = pdsAuditService;
            this.documentService = documentService;
            this.meshService = meshService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.pdsConfiguration = pdsConfiguration;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
            TryCatch(async () =>
            {
                return await meshService.ValidateMailboxAccessAsync();
            });

        public ValueTask<PdsAudit> PickupFileAndSendToMesh(byte[] pdsFile, string fileName) =>
        TryCatch(async () =>
        {
            ValidateConfigurationSettings();
            ValidateBlobContainers();
            ValidatePdsArgs(pdsFile, fileName);

            DateTimeOffset timeStamp = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid id = this.identifierBroker.GetIdentifier();
            Guid correlationId = this.identifierBroker.GetIdentifier();

            var meshMessage = await this.meshService.SendMessageAsync(
                   mexTo: this.pdsConfiguration.To,
                   mexWorkflowId: this.pdsConfiguration.WorkflowId,
                   fileContent: pdsFile,
                   mexSubject: string.Empty,
                   mexLocalId: correlationId.ToString(),
                   mexFileName: fileName,
                   mexContentChecksum: string.Empty,
                   contentType: "text/plain",
                   contentEncoding: string.Empty,
                   accept: "application/json");

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

        public ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage() =>
        TryCatch(async () =>
        {
            ValidateConfigurationSettings();
            ValidateBlobContainers();

            List<string> messageIds;
            var pdsAudits = new List<PdsAudit>();

            var exceptions = new List<Exception>();

            while ((messageIds = await this.meshService.RetrieveMessageIdsFromInboxAsync()).Count > 0)
            {
                foreach (var id in messageIds)
                {
                    try
                    {
                        PdsAudit pdsAudit = await TryCatch(async () =>
                        {
                            var message = await this.meshService.RetrieveMessageByIdAsync(id);

                            if (message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.WorkflowId)
                            {
                                return null;
                            }

                            string filename = message.Headers["mex-filename"].FirstOrDefault();
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                            string[] fileNameParts = fileNameWithoutExtension?.Split('_') ?? Array.Empty<string>();

                            string fileNameOutput =
                                $"{fileNameParts[1]}_{fileNameParts[2]}_{fileNameParts[0]}_{fileNameParts[3]}";

                            fileNameOutput += Path.GetExtension(filename);

                            var document = new Models.Foundations.Documents.Document
                            {
                                FileName = $"{pdsConfiguration.OutputFolder}/{fileNameOutput}",
                                DocumentData = message.FileContent,
                            };

                            await this.documentService.AddDocumentAsync(document, blobContainers.Pds);
                            var correlationId = Guid.Parse(message.Headers["mex-localid"].FirstOrDefault());
                            var fileName = message.Headers["mex-filename"].FirstOrDefault();
                            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

                            var pdsAudit = new PdsAudit
                            {
                                Id = this.identifierBroker.GetIdentifier(),
                                CorrelationId = correlationId,
                                FileName = document.FileName,
                                Message = $"Received message from mesh with id {message.MessageId}",
                                MessageId = message.MessageId,
                                CreatedDate = currentDate,
                                UpdatedDate = currentDate,
                                CreatedBy = "System",
                                UpdatedBy = "System"
                            };

                            await this.pdsAuditService.AddPdsAuditAsync(pdsAudit);
                            
                            return pdsAudit;
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
