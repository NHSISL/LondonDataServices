// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Orchestrations.AddressToUprns;
using LHDS.Core.Services.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Services.Foundations.Assigns;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationService : IAddressToUprnOrchestrationService
    {
        private readonly IAssignService assignService;
        private readonly IDocumentService documentService;
        private readonly AddressToUprnFileLogService addressToUprnFileLogService;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly AddressToUprnConfiguration addressToUprnConfiguration;
        private readonly BlobContainers blobContainers;

        public AddressToUprnOrchestrationService(
            IAssignService assignService,
            IDocumentService documentService,
            AddressToUprnFileLogService addressToUprnFileLogService,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker,
            IIdentifierBroker identifierBroker,
            AddressToUprnConfiguration addressToUprnConfiguration,
            BlobContainers blobContainers)
        {
            this.assignService = assignService;
            this.documentService = documentService;
            this.addressToUprnFileLogService = addressToUprnFileLogService;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.auditBroker = auditBroker;
            this.loggingBroker = loggingBroker;
            this.identifierBroker = identifierBroker;
            this.addressToUprnConfiguration = addressToUprnConfiguration;
            this.blobContainers = blobContainers;
        }

        public ValueTask MatchAddressToUprnAsync(
            Stream input,
            string fileName,
            Guid correlationId,
            CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {

            // at the start of the process create a timer to capture processing time.
            // we will use this to populate the AddressToUprnFileLog record at the end of the process and then commit that to the database.

            // Read the stream line by line via the CSV helper broker to avoid loading the entire file into memory.  This will allow us to process files of any size, even those larger than available memory.
            // The input stream is a single column CSV with a header row "UnstructuredAddress",
            // and each subsequent row contains an unstructured address to be matched to a UPRN.

            // Use the parse each line to create an AddressToUprnRequest object.
            // Send the unstructured address to the assign service to match it to a UPRN.

            // Assign will respond with an AssignAddress object.  We need to map this to a flat object AddressToUprnCsv.
            // UnstructuredAddress

            // Use the CsvHelperBroker to stream the results to a new CSV file with the above columns to the document service for storage in blob storage.
            // Use the original filename, append "_matched" to the filename and save the file to blob storage for later review.  blobContainers.AddressToUprn + addressToUprnConfiguration.Outbox

            // Exceptions /  Error handling:
            // Errors must be written to a CSV file with the same filename as the input file, but with "_error" appended to the filename. blobContainers.AddressToUprn + addressToUprnConfiguration.Error
            // Columns should be Error, CorrelationId, Message

            // If the file is not a valid CSV => Invalid CSV, %CorrelationId%, %ExceptionMessage% (as single line string, guard against commas in the strings)
            // If the file is too large to process => File too large, %CorrelationId%, %ExceptionMessage% (as single line string, guard against commas in the strings)

            // If any individual address fails i.e. timeout from assign service TimeOut, %CorrelationId%, Failed to process `%UnstructuredAddress%`. %ExceptionMessage% (as single line string, guard against commas in the strings)
            // IMPORTANT: Unmatched addresses are not errors.


            //AddressToUprnFileLog has property SuccessStatus which should be set to Failed on failure to read the file or if the size limit is exceeded.
            //For individual address failures i.e. timeout, we should still process the entire file, but the overall SuccessStatus in the AddressToUprnFileLog should be set to PartialSuccess.
        });
    }
}
