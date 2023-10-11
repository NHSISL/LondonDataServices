// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Downloads
{
    public partial class DownloadProcessingService
    {
        private delegate ValueTask<Document> ReturningDownloadFunction();

        private async ValueTask<Document> TryCatch(ReturningDownloadFunction returningDownloadFunction)
        {
            try
            {
                return await returningDownloadFunction();
            }
            catch (InvalidArgumentDownloadProcessingException invalidArgumentDownloadProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentDownloadProcessingException);
            }
        }

        private DownloadProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var downloadValidationException = new DownloadProcessingValidationException(
                message: "Download validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(downloadValidationException);

            return downloadValidationException;
        }
    }
}