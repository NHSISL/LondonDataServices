using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class AlreadyExistsAuditException : Xeption
    {
        public AlreadyExistsAuditException(Exception innerException)
            : base(message: "Audit with the same Id already exists.", innerException)
        { }
    }
}