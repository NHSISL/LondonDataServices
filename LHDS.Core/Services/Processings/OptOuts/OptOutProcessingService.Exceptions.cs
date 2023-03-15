// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService
    {
        private delegate ValueTask<OptOut> ReturningOptOutFunction();

        private async ValueTask<OptOut> TryCatch(ReturningOptOutFunction returningOptOutFunction)
        {
            try
            {
                return await returningOptOutFunction();
            }
            catch (NullOptOutProcessingException nullOptOutProcessingException)
            {
                throw CreateAndLogValidationException(nullOptOutProcessingException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutValidationException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutDependencyValidationException);
            }
        }

        private OptOutProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var optOutProcessingValidationException =
                new OptOutProcessingValidationException(exception);

            this.loggingBroker.LogError(optOutProcessingValidationException);

            return optOutProcessingValidationException;
        }

        private OptOutProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var optOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(optOutProcessingDependencyValidationException);

            return optOutProcessingDependencyValidationException;
        }
    }
}

