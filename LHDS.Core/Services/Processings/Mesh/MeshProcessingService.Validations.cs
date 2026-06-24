// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService
    {
        public void ValidateMeshArgs(string MessageId)
        {
            Validate<InvalidMeshProcessingArgumentException>(
                message: "Invalid mesh processing argument. Please correct the errors and try again.",
                (Rule: IsInvalid(MessageId), Parameter: nameof(MessageId)));
        }

        private static void ValidateMeshMessageIsNotNull(MeshMessage meshMessage)
        {
            if (meshMessage is null)
            {
                throw new NullMeshMessageProcessingException(
                    message: "Mesh processing service exception. Message is Null.");
            }
        }

        public void ValidateMeshMessageOnSendMessage(string mexTo, string workflowId, Stream content)
        {
            Validate<InvalidMeshProcessingArgumentException>(
                message: "Invalid mesh processing argument. Please correct the errors and try again.",
                (Rule: IsInvalid(mexTo), Parameter: "MexTo"),
                (Rule: IsInvalid(workflowId), Parameter: "MexWorkflowId"),
                (Rule: IsInvalidForRead(content), Parameter: "Content"));
        }

        private static void ValidateSendMessage(MeshMessage message)
        {
            ValidateMeshMessageIsNotNull(message);

            Validate<InvalidMeshMessageProcessingException>(
               message: "Invalid mesh processing argument. Please correct the errors and try again.",
               (Rule: IsInvalid(message.MessageId), Parameter: nameof(message.MessageId)));
        }

        public void ValidateMeshMessage(MeshMessage meshMessage)
        {
            ValidateMeshMessageIsNotNull(meshMessage);
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidForRead(Stream stream) => new
        {
            Condition = stream == null || !stream.CanRead || stream.Length == 0,
            Message = "Stream must be readable and contain data"
        };

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
