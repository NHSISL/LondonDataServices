using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class NullDownloadException : Xeption
    {
        public NullDownloadException()
            : base(message: "Download is null.")
        { }
    }
}