// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.DataSets
{
    internal partial class DataSetService
    {

        private delegate ValueTask<IQueryable<DataSet>> ReturningDataSetsFunction();

        private async ValueTask<IQueryable<DataSet>> TryCatch(ReturningDataSetsFunction returningDataSetsFunction)
        {
            try
            {
                return await returningDataSetsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetServiceException =
                    new FailedDataSetServiceException(
                        message: "Failed dataSet service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetServiceException);
            }
        }

        private DataSetDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var dataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCriticalAsync(dataSetDependencyException);

            return dataSetDependencyException;
        }

        private DataSetServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var dataSetServiceException =
                new DataSetServiceException(
                    message: "DataSet service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(dataSetServiceException);

            return dataSetServiceException;
        }
    }
}