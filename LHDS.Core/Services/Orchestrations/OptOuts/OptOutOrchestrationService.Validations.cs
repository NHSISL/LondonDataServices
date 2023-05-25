// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService
    {
        private static void ValidateOptOutFileIsNotNull(byte[] optOutFile)
        {
            Validate<InvalidArgumentOptOutOrchestrationException>((Rule: IsInvalid(optOutFile), Parameter: "OptOutFile"));
        }

        private static void ValidateRequestIdIsNotNull(string requestId)
        {
            Validate<InvalidArgumentOptOutOrchestrationException>((Rule: IsInvalid(requestId), Parameter: "RequestId"));
        }

        private void ValidateConfigurationSettings()
        {
            this.ValidateConfigurationIsNotNull();

            Validate<InvalidConfigOptOutOrchestrationException>(
                (Rule: IsInvalid(this.optOutConfiguration.OutputFolder),
                    Parameter: nameof(OptOutConfiguration.OutputFolder)),

                (Rule: IsInvalid(this.optOutConfiguration.ExpiredAfterDays),
                    Parameter: nameof(OptOutConfiguration.ExpiredAfterDays)));
        }

        private void ValidateConfigurationIsNotNull()
        {
            if (this.optOutConfiguration is null)
            {
                throw new NullConfigOptOutOrchestrationException();
            }
        }

        private static void ValidateLocalIdHeaderExists(MeshMessage message)
        {
            Validate<InvalidMeshMessageOrchestrationException>(
                (Rule: IsInvalidHeader(message, "Mex-LocalID"), Parameter: "Mex-LocalID"));
        }

        private static void ValidateBacthReferenceExists(string batchReference)
        {
            Validate<InvalidArgumentOptOutOrchestrationException>(
                (Rule: IsInvalid(batchReference), Parameter: "BatchReference"));
        }

        private static void ValidateDocumentRequirements(string content, string fileName)
        {
            Validate<InvalidArgumentOptOutOrchestrationException>(
                (Rule: IsInvalid(content), Parameter: "Content"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(int value) => new
        {
            Condition = value < 7,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(byte[] data) => new
        {
            Condition = data == null || data.Length == 0,
            Message = "Data is required"
        };

        private static dynamic IsInvalid(Dictionary<string, List<string>> dictionary, string key) => new
        {
            Condition = IsInvalidKey(dictionary, key),
            Message = "Header value is required"
        };

        private static dynamic IsInvalidHeader(MeshMessage message, string keyToFind) => new
        {
            Condition = IsHeaderIsInValid(message, keyToFind),
            Message = "Header value is required"
        };

        private static bool IsHeaderIsInValid(MeshMessage message, string keyToFind)
        {
            foreach (var key in message.Headers.Keys)
            {
                if (key.ToLower() == keyToFind.ToLower())
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsInvalidKey(Dictionary<string, List<string>> dictionary, string key)
        {
            bool keyExists = dictionary.ContainsKey(key);

            if (!keyExists)
            {
                return true;
            }

            string value = dictionary[key].FirstOrDefault();

            return String.IsNullOrWhiteSpace(value);
        }

        private static void Validate<T>(params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T)Activator.CreateInstance(typeof(T));

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
