// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
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
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly PdsConfiguration pdsConfiguration;

        public PdsOrchestrationService(
            IPdsAuditService pdsAuditService,
            IDocumentService documentService,
            IMeshService meshService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            PdsConfiguration pdsConfiguration
            )
        {
            this.pdsAuditService = pdsAuditService;
            this.documentService = documentService;
            this.meshService = meshService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.pdsConfiguration = pdsConfiguration;
        }

        public async ValueTask<PdsAudit> PickupFileAndSendToMesh(byte[] pdsFile, string fileName)
        {
            //ValidateAgs

            DateTimeOffset timeStamp = this.dateTimeBroker.GetCurrentDateTimeOffset();
            Guid id = this.identifierBroker.GetIdentifier();
            Guid correlationId = this.identifierBroker.GetIdentifier();

            var meshMessage = await this.meshService.SendMessageAsync(
                   mexTo: this.pdsConfiguration.To,
                   mexWorkflowId: this.pdsConfiguration.WorkflowId,
                   fileContent: pdsFile,
                   mexSubject: string.Empty,
                   mexLocalId: this.identifierBroker.GetIdentifier().ToString(),
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
                        Message = $"Sent message to mesh with id {meshMessage.MessageId}",
                        CreatedDate = timeStamp,
                        UpdatedDate = timeStamp,
                        CreatedBy = "System",
                        UpdatedBy = "System"
                    });

            return pdsAuditItem;
        }

        public ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage() =>
            throw new NotImplementedException();
    }
}
