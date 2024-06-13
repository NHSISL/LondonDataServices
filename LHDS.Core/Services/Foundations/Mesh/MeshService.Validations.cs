// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService
    {
        public void ValidateMeshArgs(string messageId)
        {
            Validate<InvalidMeshMessageException>(
                message: "Invalid mesh message, please correct errors and try again.",
                (Rule: IsInvalid(messageId), Parameter: "MessageId"));
        }

        public void ValidateMeshMessageOnSendMessage(string mexTo, string workflowId, byte[] fileContent)
        {
            Validate<InvalidMeshMessageException>(
                message: "Invalid mesh message, please correct errors and try again.",
                (Rule: IsInvalid(mexTo), Parameter: "MexTo"),
                (Rule: IsInvalid(workflowId), Parameter: "MexWorkflowId"),
                (Rule: IsInvalid(fileContent), Parameter: "FileContent"));
        }

        public void ValidateMessageId(string messageId) =>
            Validate<InvalidArgumentMeshException>(
                message: "Invalid Mesh argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(messageId), Parameter: "MessageId"));

        public void ValidateMeshMessageIsNotNull(MeshMessage message)
        {
            if (message is null)
            {
                throw new NullMeshMessageException(message: "Mesh message is null.");
            }
        }

        private static void ValidateHeadersIsNotNull(MeshMessage message)
        {
            if (message.Headers is null)
            {
                throw new NullHeadersException(message: "Mesh message headers dictionary is null.");
            }
        }

        private static dynamic IsInvalid(string? text) => new
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

            string? value = dictionary[key].FirstOrDefault();

            return String.IsNullOrWhiteSpace(value);
        }

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T?)Activator.CreateInstance(typeof(T), message);

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException?.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException?.ThrowIfContainsErrors();
        }
    }
}
