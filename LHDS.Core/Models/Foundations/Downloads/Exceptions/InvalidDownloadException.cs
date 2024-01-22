// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class InvalidDownloadException : Xeption
    {
        public InvalidDownloadException(string message)
            : base(message)
        { }
    }
}