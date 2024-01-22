// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CsvMappers.Exceptions
{
    public class CsvMapperProcessingValidationException : Xeption
    {
        private const string validationMessage = "Csv Mapper processing validation errors occured, please try again";

        public CsvMapperProcessingValidationException(Xeption innerException, string validationSummary = "")
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException)
        { }
    }
}
