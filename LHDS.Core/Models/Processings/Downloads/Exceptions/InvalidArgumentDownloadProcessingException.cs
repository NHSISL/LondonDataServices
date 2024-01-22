// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Downloads.Exceptions
{
    public class InvalidArgumentDownloadProcessingException : Xeption
    {
        public InvalidArgumentDownloadProcessingException(string message)
            : base(message)
        { }
    }
}