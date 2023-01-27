// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class DownloadDependencyValidationException : Xeption
    {
        public DownloadDependencyValidationException(Xeption innerException)
            : base(message: "Download dependency validation occurred, please try again.", innerException)
        { }
    }
}