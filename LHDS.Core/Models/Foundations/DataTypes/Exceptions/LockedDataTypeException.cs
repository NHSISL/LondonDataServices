using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class LockedDataTypeException : Xeption
    {
        public LockedDataTypeException(Exception innerException)
            : base(message: "Locked dataType record exception, please try again later", innerException)
        {
        }
    }
}