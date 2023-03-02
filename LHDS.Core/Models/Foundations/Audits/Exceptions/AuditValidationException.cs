// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class AuditValidationException : Xeption
    {
        private const string validationMessage = "Audit validation errors occurred, please try again.";

        public AuditValidationException(Xeption innerException)
            : base(message: validationMessage,
                  innerException)
        { }

        public AuditValidationException(Xeption innerException, string validationSummary)
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException)
        { }
    }
}