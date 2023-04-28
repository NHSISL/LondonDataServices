// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CsvMapper.Exceptions
{
    public class CsvMapperProcessingDependencyException : Xeption
    {
        public CsvMapperProcessingDependencyException(Xeption innerException) :
            base(message: "Csv Mapper processing dependency error occurred, contact support.", innerException)
        { }
    }
}
