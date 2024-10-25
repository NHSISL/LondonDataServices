using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class ObjectColumnValidationException : Xeption
    {
        public ObjectColumnValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}