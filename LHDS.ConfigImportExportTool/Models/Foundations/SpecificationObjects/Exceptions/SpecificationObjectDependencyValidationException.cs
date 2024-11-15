using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class SpecificationObjectDependencyValidationException : Xeption
    {
        public SpecificationObjectDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}