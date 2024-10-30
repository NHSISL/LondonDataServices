// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions;

namespace LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns
{
    internal partial class ObjectColumnProcessingService
    {
        private static void ValidateObjectColumnProcessingOnRetrieveOrAdd(
            ObjectColumn ObjectColumn)
        {
            ValidateObjectColumnProcessingIsNotNull(ObjectColumn);
        }

        private static void ValidateObjectColumnProcessingOnModify(ObjectColumn ObjectColumn)
        {
            ValidateObjectColumnProcessingIsNotNull(ObjectColumn);
        }

        private static void ValidateObjectColumnProcessingIsNotNull(ObjectColumn ObjectColumn)
        {
            if (ObjectColumn is null)
            {
                throw new NullObjectColumnProcessingException(
                    message: "Specification object processing is Null");
            }
        }
    }
}
