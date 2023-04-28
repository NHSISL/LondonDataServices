// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CsvMappers.Exceptions
{
    public class CsvMapperDependencyException : Xeption
    {
        public CsvMapperDependencyException(Xeption innerException) :
            base(message: "CSV mapper dependency error occurred, contact support.", innerException)
        { }
    }
}
