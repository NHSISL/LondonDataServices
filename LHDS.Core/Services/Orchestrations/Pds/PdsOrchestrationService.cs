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

        public ValueTask<PdsAudit> PickupFileAndSendToMesh(byte[] pdsFile, string fileName) =>
            throw new NotImplementedException();

        public ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage() =>
            throw new NotImplementedException();
    }
}
