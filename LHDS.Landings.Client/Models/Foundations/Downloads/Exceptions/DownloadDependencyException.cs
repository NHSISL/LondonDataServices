// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class DownloadDependencyException : Xeption
    {
        public DownloadDependencyException(Xeption innerException) :
            base(message: "Download dependency error occurred, contact support.", innerException)
        { }
    }
}