using LHDS.Landings.Client.Models.Downloads;
using LHDS.Landings.Client.Models.Downloads.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private void ValidateDownloadOnAdd(Download download)
        {
            ValidateDownloadIsNotNull(download);
        }

        private static void ValidateDownloadIsNotNull(Download download)
        {
            if (download is null)
            {
                throw new NullDownloadException();
            }
        }
    }
}