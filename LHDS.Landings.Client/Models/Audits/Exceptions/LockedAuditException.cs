using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class LockedAuditException : Xeption
    {
        public LockedAuditException(Exception innerException)
            : base(message: "Locked audit record exception, please try again later", innerException)
        {
        }
    }
}