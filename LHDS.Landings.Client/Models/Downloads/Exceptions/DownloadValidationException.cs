using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class DownloadValidationException : Xeption
    {
        public DownloadValidationException(Xeption innerException)
            : base(message: "Download validation errors occurred, please try again.",
                  innerException)
        { }
    }
}