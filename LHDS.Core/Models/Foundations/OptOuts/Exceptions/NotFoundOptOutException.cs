using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class NotFoundOptOutException : Xeption
    {
        public NotFoundOptOutException(Guid optOutId)
            : base(message: $"Couldn't find optOut with optOutId: {optOutId}.")
        { }
    }
}