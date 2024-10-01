using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class SpecificationObjectDependencyException : Xeption
    {
        public SpecificationObjectDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}