// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingService : IObjectColumnProcessingService
    {
        private delegate ValueTask<ObjectColumn> ReturningObjectColumnProcessingFunction();

        private async ValueTask<ObjectColumn> TryCatch(ReturningObjectColumnProcessingFunction returningObjectColumnProcessingFunction)
        {
            try
            {
                return await returningObjectColumnProcessingFunction();
            }
            catch (NullObjectColumnProcessingException nullObjectColumnException)
            {
                throw CreateAndLogValidationException(nullObjectColumnException);
            }
        }

        private ObjectColumnProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var objectColumnProcessingValidationExceptionn =
                new ObjectColumnProcessingValidationException(
                    message: "ObjectColumn processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnProcessingValidationExceptionn);

            return objectColumnProcessingValidationExceptionn;
        }
    }
}
