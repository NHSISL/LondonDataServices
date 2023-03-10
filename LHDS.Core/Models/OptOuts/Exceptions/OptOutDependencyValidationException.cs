using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class OptOutDependencyValidationException : Xeption
    {
        public OptOutDependencyValidationException(Xeption innerException)
            : base(message: "OptOut dependency validation occurred, please try again.", innerException)
        { }
    }
}