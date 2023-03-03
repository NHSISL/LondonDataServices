// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class NotFoundDownloadException : Xeption
    {
        public NotFoundDownloadException(string fileName)
            : base(message: $"Couldn't find download with file name: {fileName}.") { }
    }
}