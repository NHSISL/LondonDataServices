using System.Threading.Tasks;
using LHDS.Core.Models.OptOuts;
using LHDS.Core.Models.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService
    {
        private delegate ValueTask<OptOut> ReturningOptOutFunction();

        private async ValueTask<OptOut> TryCatch(ReturningOptOutFunction returningOptOutFunction)
        {
            try
            {
                return await returningOptOutFunction();
            }
            catch (NullOptOutException nullOptOutException)
            {
                throw CreateAndLogValidationException(nullOptOutException);
            }
        }

        private OptOutValidationException CreateAndLogValidationException(Xeption exception)
        {
            var optOutValidationException =
                new OptOutValidationException(exception);

            this.loggingBroker.LogError(optOutValidationException);

            return optOutValidationException;
        }
    }
}