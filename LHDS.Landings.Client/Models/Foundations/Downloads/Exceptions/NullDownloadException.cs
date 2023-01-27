using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class NullDownloadException : Xeption
    {
        public NullDownloadException()
            : base(message: "Download is null.")
        { }
    }
}