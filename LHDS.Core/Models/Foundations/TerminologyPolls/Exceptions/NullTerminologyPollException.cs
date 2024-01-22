using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class NullTerminologyPollException : Xeption
    {
        public NullTerminologyPollException(string message)
            : base(message)
        { }
    }
}