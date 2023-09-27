// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationService
    {
        public void ValidatePdsArgs(byte[] pdsFile, string fileName)
        {
            Validate<InvalidArgumentPdsException>(
                message: "Invalid PDS argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(pdsFile), Parameter: "pdsFile"),
                (Rule: IsInvalid(fileName), Parameter: "fileName"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(byte[] data) => new
        {
            Condition = data == null || data.Length == 0,
            Message = "Data is required"
        };

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T)Activator.CreateInstance(typeof(T), message);

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}
