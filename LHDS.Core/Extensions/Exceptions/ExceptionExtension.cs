// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHDS.Core.Extensions.Exceptions
{
    public static class ExceptionExtension
    {
        public static string GetValidationSummary(this Exception exception)
        {
            if ((exception == null || exception.Data.Count == 0)
                && (exception?.InnerException == null || exception.InnerException.Data.Count == 0))
            {
                return string.Empty;
            }

            StringBuilder validationSummary = new StringBuilder();
            validationSummary.Append(GetErrorSummary(exception));
            validationSummary.Append(GetErrorSummary(exception.InnerException));

            return validationSummary.ToString();
        }

        private static string GetErrorSummary(Exception exception)
        {
            StringBuilder validationSummary = new StringBuilder();

            if (exception != null && exception.Data.Count > 0)
            {
                validationSummary.Append($"{exception.GetType().Name} Errors:  ");

                StringBuilder errors = new StringBuilder();

                foreach (DictionaryEntry entry in exception.Data)
                {
                    string errorSummary = ((List<string>)entry.Value)
                        .Select((string value) => value)
                        .Aggregate((string current, string next) => current + ", " + next);

                    errors.Append($"{entry.Key} => {errorSummary};  ");
                }

                validationSummary.Append(errors.ToString().Trim() + Environment.NewLine);
            }

            return validationSummary.ToString();
        }
    }
}
