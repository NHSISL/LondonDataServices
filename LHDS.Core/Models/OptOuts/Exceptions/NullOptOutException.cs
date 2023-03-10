using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class NullOptOutException : Xeption
    {
        public NullOptOutException()
            : base(message: "OptOut is null.")
        { }
    }
}