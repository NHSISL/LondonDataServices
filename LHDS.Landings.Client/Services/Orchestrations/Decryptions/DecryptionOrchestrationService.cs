// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHDS.Landings.Client.Services.Orchestrations.Decryptions
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using global::LHDS.Landings.Client.Brokers.DateTimes;
    using global::LHDS.Landings.Client.Brokers.Loggings;
    using global::LHDS.Landings.Client.Models.Foundations.Documents;
    using global::LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
    using global::LHDS.Landings.Client.Services.Foundations.Audits;
    using global::LHDS.Landings.Client.Services.Foundations.Decryptions;
    using global::LHDS.Landings.Client.Services.Foundations.Documents;
    using global::LHDS.Landings.Client.Services.Foundations.IngestionTrackings;
    using Microsoft.IdentityModel.Protocols;

    namespace LHDS.Landings.Client.Services.Foundations.Decryptions
    {
        public class DecryptionOrchestrationService : IDecryptionOrchestrationService
        {
            private readonly IDocumentService documentService;
            private readonly IDecryptionService decryptionService;
            private readonly IIngestionTrackingService ingestionTrackingService;
            private readonly IAuditService auditService;
            private readonly ILoggingBroker loggingBroker;
            private readonly IDateTimeBroker dateTimeBroker;

            public DecryptionOrchestrationService(
                IDocumentService documentService,
                IDecryptionService decryptionService,
                IIngestionTrackingService ingestionTrackingService,
                IAuditService auditService,
                ILoggingBroker loggingBroker,
                IDateTimeBroker dateTimeBroker)
            {
                this.documentService = documentService;
                this.decryptionService = decryptionService;
                this.ingestionTrackingService = ingestionTrackingService;
                this.auditService = auditService;
                this.loggingBroker = loggingBroker;
                this.dateTimeBroker = dateTimeBroker;
            }

            public async ValueTask DecryptAsync(string fileName)
            {
                Document retrievedDocument =
                await this.documentService.RetrieveDocumentByFileNameAsync(fileName, false);

                byte[] 

                byte[] randomDecryption =
                    await this.decryptionService.DecryptAsync(randomDecryption);

                var 

            
                IngestionTracking maybeIngestionTracking =
                    await this.ingestionTrackingService
                        .RetrieveIngestionTrackingByFileNameAsync(document.FileName);

                if (maybeIngestionTracking == null)
                {
                    Document retrievedDocument =
                        await this.downloadService.RetrieveDownloadByFileNameAsync(document.FileName);

                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    IngestionTracking newIngestionTracking =
                        new IngestionTracking
                        {
                            Id = document.FileName,
                            FileName = document.FileName,
                            Decrypted = false,
                            CreatedDate = currentDateTime,
                        };

                    await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);
                    await this.documentService.AddDocumentAsync(retrievedDocument, false);
                    LogAudit(document, currentDateTime);
                }
                
                byte[] encryptedData = await documentService.RetrieveDocumentByFileNameAsync(fileName, false);
                byte[] decryptedData = await auditService.DecryptAsync(encryptedData);
                await documentService.AddDocumentAsync(fileName, decryptedData);
                await ingestionTrackingService.TrackAsync(fileName, decryptedData);
                await auditService.LogDecryptionAsync(fileName, decryptedData);
            }
        }
    }

}
