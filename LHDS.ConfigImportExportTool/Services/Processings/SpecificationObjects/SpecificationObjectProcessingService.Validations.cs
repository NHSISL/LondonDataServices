// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions;

namespace LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects
{
    internal partial class SpecificationObjectProcessingService
    {
        private static void ValidateSpecificationObjectProcessingOnRetrieveOrAdd(
            SpecificationObject specificationObject)
        {
            ValidateSpecificationObjectProcessingIsNotNull(specificationObject);
        }

        private static void ValidateSpecificationObjectProcessingOnModify(SpecificationObject specificationObject)
        {
            ValidateSpecificationObjectProcessingIsNotNull(specificationObject);
        }

        private static void ValidateSpecificationObjectProcessingIsNotNull(SpecificationObject specificationObject)
        {
            if (specificationObject is null)
            {
                throw new NullSpecificationObjectProcessingException(
                    message: "Specification object processing is Null");
            }
        }
    }
}
