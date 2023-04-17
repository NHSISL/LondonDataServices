// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService
    {
        public void ValidateMeshArgs(string mailboxId, string messageId)
        {
            Validate(
                (Rule: IsInvalid(mailboxId), Parameter: nameof(mailboxId)),
                (Rule: IsInvalid(messageId), Parameter: nameof(messageId)));
        }

        public void ValidateMeshMessageOnSendMessage(MeshMessage message)
        {
            ValidateMeshMessageIsNotNull(message);

            Validate<InvalidMeshMessageException>(
                (Rule: IsInvalid(message.StringContent), Parameter: nameof(message.StringContent)),
                (Rule: IsInvalid(message.Headers), Parameter: nameof(message.Headers)));

            Validate<InvalidMeshMessageException>(
                (Rule: IsInvalid(message.StringContent), Parameter: nameof(message.StringContent)),
                (Rule: IsInvalid(message.Headers, "Content-Type"), Parameter: "Content-Type"),
                (Rule: IsInvalid(message.Headers, "Mex-FileName"), Parameter: "Mex-FileName"),
                (Rule: IsInvalid(message.Headers, "Mex-From"), Parameter: "Mex-From"),
                (Rule: IsInvalid(message.Headers, "Mex-To"), Parameter: "Mex-To"),
                (Rule: IsInvalid(message.Headers, "Mex-WorkflowID"), Parameter: "Mex-WorkflowID"));
        }

        public void ValidateMessageId(string messageId) =>
            Validate((Rule: IsInvalid(messageId), Parameter: nameof(messageId)));

        public void ValidateMailboxId(string mailboxId) =>
          Validate((Rule: IsInvalid(mailboxId), Parameter: nameof(mailboxId)));

        public void ValidateMeshMessageIsNotNull(MeshMessage message)
        {
            if (message is null)
            {
                throw new NullMeshMessageException();
            }
        }

        private static void ValidateHeadersIsNotNull(MeshMessage message)
        {
            if (message.Headers is null)
            {
                throw new NullHeadersException();
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Dictionary<string, List<string>> dictionary) => new
        {
            Condition = dictionary == null,
            Message = "Values is required"
        };

        private static dynamic IsInvalid(Dictionary<string, List<string>> dictionary, string key) => new
        {
            Condition = IsInvalidKey(dictionary, key),
            Message = "Header value is required"
        };

        private static dynamic IsInvalid(byte[] data) => new
        {
            Condition = (data == null || data.Length == 0),
            Message = "Content is required"
        };

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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentMeshException = new InvalidArgumentMeshException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentMeshException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentMeshException.ThrowIfContainsErrors();
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
