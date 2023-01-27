// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class DownloadValidationException : Xeption
    {
        public DownloadValidationException(Xeption innerException)
            : base(message: "Download validation errors occurred, please try again.",
                  innerException)
        { }
    }
}