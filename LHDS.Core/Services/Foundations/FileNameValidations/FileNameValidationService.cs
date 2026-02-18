// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationService : IFileNameValidationService
    {
        public FileNameValidationService() { }

        public ValueTask<bool> ShouldProcessFileAsync(
            string fileName,
            string? includePattern,
            string? excludePattern) =>
        TryCatch(async () =>
        {
            ValidateArguments(fileName);

            if (!string.IsNullOrWhiteSpace(includePattern))
            {
                if (!Regex.IsMatch(fileName, includePattern))
                {
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(excludePattern))
            {
                bool isExcluded = Regex.IsMatch(fileName, excludePattern);

                if (isExcluded)
                {
                    return false;
                }
            }

            return true;
        });
    }
}