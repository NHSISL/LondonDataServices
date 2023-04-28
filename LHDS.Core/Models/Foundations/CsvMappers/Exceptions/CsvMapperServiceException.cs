// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CsvMappers.Exceptions
{
    public class CsvMapperServiceException : Xeption
    {
        public CsvMapperServiceException(Xeption innerException)
          : base(message: "CSV mapper service error occurred, contact support.",
                innerException)
        { }
    }
}
