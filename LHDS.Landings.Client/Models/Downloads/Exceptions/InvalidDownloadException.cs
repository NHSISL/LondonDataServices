using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class InvalidDownloadException : Xeption
    {
        public InvalidDownloadException()
            : base(message: "Invalid download. Please correct the errors and try again.")
        { }
    }
}