// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class DownloadValidationException : Xeption
    {
        public DownloadValidationException(string message, Xeption? innerException)
            : base(
                message: "Download validation errors occurred, please try again.",
                innerException)
        { }
    }
}