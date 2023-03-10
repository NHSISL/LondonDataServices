using LHDS.Core.Models.OptOuts;
using LHDS.Core.Models.OptOuts.Exceptions;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService
    {
        private void ValidateOptOutOnAdd(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);
        }

        private static void ValidateOptOutIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutException();
            }
        }
    }
}