// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Downloads.Exceptions
{
    public class NullDownloadProcessingException : Xeption
    {
        public NullDownloadProcessingException(string message)
            : base(message)
        { }
    }
}