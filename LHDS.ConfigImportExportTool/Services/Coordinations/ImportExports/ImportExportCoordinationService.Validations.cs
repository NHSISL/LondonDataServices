// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports
{
    internal partial class ImportExportCoordinationService
    {
        private void ValidateImportFileArguments(string dataSetName, string version, string filePath)
        {
            Validate<InvalidArgumentImportExportCoordinationException>(
                message: "Invalid import export coordination argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(dataSetName), Parameter: nameof(dataSetName)),
                (Rule: IsInvalid(version), Parameter: nameof(version)),
                (Rule: IsInvalid(filePath), Parameter: nameof(filePath)));
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
