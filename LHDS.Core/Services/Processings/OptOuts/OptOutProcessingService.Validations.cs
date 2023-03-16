// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Collections;
using System.Text;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using LHDS.Core.Services.Foundations.OptOuts;
using System.Linq;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService
    {
        private static void ValidateOptOutProcessingOnRetrieveOrAdd(OptOut optOut)
        {
            ValidateOptOutProcessingIsNotNull(optOut);
        }

        private static void ValidateOptOutProcessingOnModify(OptOut optOut)
        {
            ValidateOptOutProcessingIsNotNull(optOut);
        }

        private static void ValidateOptOutProcessingIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutProcessingException();
            }
        }

        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}

