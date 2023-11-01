using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapService
    {
        private delegate ValueTask<OntologyConceptMap> ReturningOntologyConceptMapFunction();

        private async ValueTask<OntologyConceptMap> TryCatch(ReturningOntologyConceptMapFunction returningOntologyConceptMapFunction)
        {
            try
            {
                return await returningOntologyConceptMapFunction();
            }
            catch (NullOntologyConceptMapException nullOntologyConceptMapException)
            {
                throw CreateAndLogValidationException(nullOntologyConceptMapException);
            }
            catch (InvalidOntologyConceptMapException invalidOntologyConceptMapException)
            {
                throw CreateAndLogValidationException(invalidOntologyConceptMapException);
            }
            catch (SqlException sqlException)
            {
                var failedOntologyConceptMapStorageException =
                    new FailedOntologyConceptMapStorageException(
                        message: "Failed ontologyConceptMap storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOntologyConceptMapStorageException);
            }
        }

        private OntologyConceptMapValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyConceptMapValidationException);

            return ontologyConceptMapValidationException;
        }

        private OntologyConceptMapDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ontologyConceptMapDependencyException = 
                new OntologyConceptMapDependencyException(
                    message: "OntologyConceptMap dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(ontologyConceptMapDependencyException);

            return ontologyConceptMapDependencyException;
        }
    }
}