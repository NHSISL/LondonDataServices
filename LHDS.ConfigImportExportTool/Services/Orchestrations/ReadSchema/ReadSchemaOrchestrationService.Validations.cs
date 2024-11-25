// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.ConfigImportExportTool.Models.Foundations.Files.Exceptions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema
{
    internal partial class ReadSchemaOrchestrationService
    {
        private void ValidateWriteSchemaFileArguments(List<SpecificationObject> specificationObjects, string path)
        {
            ValidateSpecificationObjectListIsNotNull(specificationObjects);

            Validate<InvalidArgumentReadSchemaOrchestrationException>(
                message: "Invalid read schema argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private void ValidateProcessSchemaFileArguments(string path)
        {
            Validate<InvalidArgumentReadSchemaOrchestrationException>(
                message: "Invalid read schema argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void ValidateSpecificationObjectListIsNotNull(List<SpecificationObject> specificationObjects)
        {
            if (specificationObjects == null)
            {
                throw new NullSpecificationObjectListException(message: "Specification object list is null.");
            }
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
