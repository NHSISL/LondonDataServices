// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class NotFoundDownloadException : Xeption
    {
        public NotFoundDownloadException(string fileName)
            : base(message: $"Couldn't find download with file name: {fileName}.")
        { }
    }
}