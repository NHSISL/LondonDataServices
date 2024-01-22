// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CsvMappers.Exceptions
{
    public class CsvMapperProcessingServiceException : Xeption
    {
        public CsvMapperProcessingServiceException(string message, Xeption innerException)
          : base(message, innerException)
        { }
    }
}
