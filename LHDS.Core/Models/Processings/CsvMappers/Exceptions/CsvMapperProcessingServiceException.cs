// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CsvMappers.Exceptions
{
    public class CsvMapperProcessingServiceException : Xeption
    {
        public CsvMapperProcessingServiceException(Xeption innerException)
          : base(message: "Csv Mapper processing service error occurred, contact support.",
                innerException)
        { }
    }
}
