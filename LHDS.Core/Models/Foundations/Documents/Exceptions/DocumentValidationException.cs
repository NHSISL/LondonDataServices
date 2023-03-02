// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class DocumentValidationException : Xeption
    {
        private const string validationMessage = "Document validation errors occured, please try again";

        public DocumentValidationException(Xeption innerException, string validationSummary = "")
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException)
        { }
    }
}
