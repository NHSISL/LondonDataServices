// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class OptOutValidationException : Xeption
    {
        private const string validationMessage = "OptOut validation errors occurred, please try again.";

        public OptOutValidationException(Xeption innerException, string validationSummary = "")
        : base(
              message: validationSummary.Length > 0
                ? $"{validationMessage}  Validation errors: {validationSummary}"
                : validationMessage,
              innerException)
        { }
    }
}