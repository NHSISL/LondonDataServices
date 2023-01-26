using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Downloads;
using LHDS.Landings.Client.Models.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private delegate ValueTask<Download> ReturningDownloadFunction();

        private async ValueTask<Download> TryCatch(ReturningDownloadFunction returningDownloadFunction)
        {
            try
            {
                return await returningDownloadFunction();
            }
            catch (NullDownloadException nullDownloadException)
            {
                throw CreateAndLogValidationException(nullDownloadException);
            }
            catch (InvalidDownloadException invalidDownloadException)
            {
                throw CreateAndLogValidationException(invalidDownloadException);
            }
        }

        private DownloadValidationException CreateAndLogValidationException(Xeption exception)
        {
            var downloadValidationException =
                new DownloadValidationException(exception);

            this.loggingBroker.LogError(downloadValidationException);

            return downloadValidationException;
        }
    }
}