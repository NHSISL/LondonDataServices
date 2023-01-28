// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class AlreadyExistsDownloadException : Xeption
    {
        public AlreadyExistsDownloadException(Exception innerException)
            : base(message: "Download with the same Id already exists.", innerException)
        { }
    }
}