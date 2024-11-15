// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Clients.Exceptions;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Clients
{
    internal class ImportExportClient : IImportExportClient
    {
        private readonly IImportExportCoordinationService importExportCoordinationService;

        public ImportExportClient(IImportExportCoordinationService importExportCoordinationService)
        {
            this.importExportCoordinationService = importExportCoordinationService;
        }

        public async ValueTask Import(string dataSetName, string version, string filePath)
        {
            try
            {
                await this.importExportCoordinationService.Import(dataSetName, version, filePath);
            }
            catch (ImportExportValidationCoordinationException ImportExportCoordinationValidationException)
            {
                throw new ImportExportClientValidationException(
                    message: "Import export client validation error occurred, fix errors and try again.",
                    innerException: ImportExportCoordinationValidationException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationDependencyValidationException
                ImportExportCoordinationDependencyValidationException)
            {
                throw new ImportExportClientValidationException(
                    message: "Import export client validation error occurred, fix errors and try again.",
                    innerException: ImportExportCoordinationDependencyValidationException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationDependencyException
                ImportExportCoordinationDependencyException)
            {
                throw new ImportExportClientDependencyException(
                    message: "Import export client dependency error occurred, please contact support.",
                    innerException: ImportExportCoordinationDependencyException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationServiceException
                ImportExportCoordinationServiceException)
            {
                throw new ImportExportClientServiceException(
                    message: "Import export client service error occurred, fix errors and try again.",
                    ImportExportCoordinationServiceException.InnerException as Xeption);
            }
        }
    }
}
