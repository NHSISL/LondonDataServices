// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class DownloadValidationException : Xeption
    {
        private const string validationMessage = "Download validation errors occurred, please try again.";

        public DownloadValidationException(Xeption innerException, string validationSummary = "")
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException)
        { }
    }
}