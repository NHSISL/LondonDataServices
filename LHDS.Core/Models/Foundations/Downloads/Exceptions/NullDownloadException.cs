// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class NullDownloadException : Xeption
    {
        public NullDownloadException(string message)
            : base(message)
        { }
    }
}