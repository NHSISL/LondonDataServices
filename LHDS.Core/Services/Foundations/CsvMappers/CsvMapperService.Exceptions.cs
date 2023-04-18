// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using Xeptions;

namespace LHDS.Core.Brokers.CsvMappers
{
    public partial class CsvMapperService : ICsvMapperService
    {
        private delegate ValueTask<T> ReturningObjectFunction<T>();

        private async ValueTask<T> TryCatch<T>(ReturningObjectFunction<T> returningObjectFunction)
        {
            try
            {
                return await returningObjectFunction();
            }
            catch (InvalidCsvMapperArgumentsException invalidCsvMapperArgumentsException)
            {
                throw CreateAndLogValidationException(invalidCsvMapperArgumentsException);
            }
        }

        private CsvMapperValidationException CreateAndLogValidationException(Xeption exception)
        {
            var csvMapperValidationException = new CsvMapperValidationException(exception);
            this.loggingBroker.LogError(csvMapperValidationException);

            return csvMapperValidationException;
        }
    }
}
