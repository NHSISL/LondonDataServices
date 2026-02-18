// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.Core.Services.Foundations.FileNameValidations
{
    public interface IFileNameValidationService
    {
        ValueTask<bool> ShouldProcessFileAsync(
            string fileName,
            string includePattern,
            string excludePattern);
    }
}