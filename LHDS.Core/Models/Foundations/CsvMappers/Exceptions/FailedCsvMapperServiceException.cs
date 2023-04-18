// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.CsvMappers.Exceptions
{
    public class FailedCsvMapperServiceException : Xeption
    {
        public FailedCsvMapperServiceException(Exception innerException)
          : base(message: "Failed CSV mapper service error occurred, contact support.",
                innerException)
        { }
    }
}
