using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class OptOutValidationException : Xeption
    {
        public OptOutValidationException(Xeption innerException)
            : base(message: "OptOut validation errors occurred, please try again.",
                  innerException)
        { }
    }
}