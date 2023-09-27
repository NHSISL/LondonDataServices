// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;

namespace LHDS.Core.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingService : IObjectColumnProcessingService
    {
        private void ValidateObjectColumn(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);
        }

        private static void ValidateObjectColumnIsNotNull(ObjectColumn objectColumn)
        {
            if (objectColumn is null)
            {
                throw new NullObjectColumnProcessingException(message: "ObjectColumn is null.");
            }
        }
    }
}
