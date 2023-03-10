using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class OptOutDependencyException : Xeption
    {
        public OptOutDependencyException(Xeption innerException) :
            base(message: "OptOut dependency error occurred, contact support.", innerException)
        { }
    }
}