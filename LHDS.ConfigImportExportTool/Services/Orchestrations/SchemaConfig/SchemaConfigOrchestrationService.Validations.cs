// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal partial class SchemaConfigOrchestrationService
    {
        private void ValidateSchemaImportArguments(
            List<SpecificationObject> specificationObjects,
            string dataSetName,
            string version)
        {
            ValidateSpecificationObjectListIsNotNull(specificationObjects);

            Validate<InvalidArgumentSchemaConfigOrchestrationException>(
                message: "Invalid schema config argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(dataSetName), Parameter: nameof(dataSetName)),
                (Rule: IsInvalid(version), Parameter: nameof(version)));
        }

        private static void ValidateSpecificationObjectListIsNotNull(List<SpecificationObject> specificationObjects)
        {
            if (specificationObjects is null)
            {
                throw new NullSpecificationObjectListException(message: "Specification object list is null.");
            }
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
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
