// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Processings.Mesh.Exceptions;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService
    {
        public void ValidateMeshArgs(string MessageId)
        {
            Validate(
                (Rule: IsInvalid(MessageId), Parameter: nameof(MessageId)));
        }

        private static void ValidateMeshMessageIsNotNull(MeshMessage meshMessage)
        {
            if (meshMessage is null)
            {
                throw new NullMeshProcessingException();
            }
        }

        public void ValidateMeshMessage(MeshMessage meshMessage)
        {
            ValidateMeshMessageIsNotNull(meshMessage);
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidMeshProcessingException = new InvalidMeshProcessingArgumentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidMeshProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidMeshProcessingException.ThrowIfContainsErrors();
        }
    }
}
