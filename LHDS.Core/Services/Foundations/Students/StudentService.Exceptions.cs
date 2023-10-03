using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.Students;
using LHDS.Core.Models.Foundations.Students.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Students
{
    public partial class StudentService
    {
        private delegate ValueTask<Student> ReturningStudentFunction();
        private delegate IQueryable<Student> ReturningStudentsFunction();

        private async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
        {
            try
            {
                return await returningStudentFunction();
            }
            catch (NullStudentException nullStudentException)
            {
                throw CreateAndLogValidationException(nullStudentException);
            }
            catch (InvalidStudentException invalidStudentException)
            {
                throw CreateAndLogValidationException(invalidStudentException);
            }
            catch (SqlException sqlException)
            {
                var failedStudentStorageException =
                    new FailedStudentStorageException(
                        message: "Failed student storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedStudentStorageException);
            }
            catch (NotFoundStudentException notFoundStudentException)
            {
                throw CreateAndLogValidationException(notFoundStudentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentException =
                    new AlreadyExistsStudentException(
                        message: "Student with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsStudentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidStudentReferenceException =
                    new InvalidStudentReferenceException(
                        message: "Invalid student reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidStudentReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedStudentStorageException =
                    new FailedStudentStorageException(
                        message: "Failed student storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedStudentStorageException);
            }
            catch (Exception exception)
            {
                var failedStudentServiceException =
                    new FailedStudentServiceException(
                        message: "Failed student service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedStudentServiceException);
            }
        }

        private IQueryable<Student> TryCatch(ReturningStudentsFunction returningStudentsFunction)
        {
            try
            {
                return returningStudentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStudentStorageException =
                    new FailedStudentStorageException(
                        message: "Failed student storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedStudentStorageException);
            }
            catch (Exception exception)
            {
                var failedStudentServiceException =
                    new FailedStudentServiceException(
                        message: "Failed student service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedStudentServiceException);
            }
        }

        private StudentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException =
                new StudentValidationException(
                    message: "Student validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }

        private StudentDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = 
                new StudentDependencyException(
                    message: "Student dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private StudentDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var studentDependencyValidationException =
                new StudentDependencyValidationException(
                    message: "Student dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private StudentDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var studentDependencyException = 
                new StudentDependencyException(
                    message: "Student dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private StudentServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var studentServiceException = 
                new StudentServiceException(
                    message: "Student service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}