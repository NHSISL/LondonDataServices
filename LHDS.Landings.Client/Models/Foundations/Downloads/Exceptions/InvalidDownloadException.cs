// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class InvalidDownloadException : Xeption
    {
        public InvalidDownloadException()
            : base(message: "Invalid download. Please correct the errors and try again.")
        { }
    }
}