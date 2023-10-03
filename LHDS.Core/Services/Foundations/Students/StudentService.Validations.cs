using System;
using LHDS.Core.Models.Foundations.Students;
using LHDS.Core.Models.Foundations.Students.Exceptions;

namespace LHDS.Core.Services.Foundations.Students
{
    public partial class StudentService
    {
        private void ValidateStudentOnAdd(Student student)
        {
            ValidateStudentIsNotNull(student);

            Validate(
                (Rule: IsInvalid(student.Id), Parameter: nameof(Student.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),
                (Rule: IsInvalid(student.CreatedBy), Parameter: nameof(Student.CreatedBy)),
                (Rule: IsInvalid(student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)),
                (Rule: IsInvalid(student.UpdatedBy), Parameter: nameof(Student.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: student.UpdatedDate,
                    secondDate: student.CreatedDate,
                    secondDateName: nameof(Student.CreatedDate)),
                Parameter: nameof(Student.UpdatedDate)),

                (Rule: IsNotSame(
                    first: student.UpdatedBy,
                    second: student.CreatedBy,
                    secondName: nameof(Student.CreatedBy)),
                Parameter: nameof(Student.UpdatedBy)),

                (Rule: IsNotRecent(student.CreatedDate), Parameter: nameof(Student.CreatedDate)));
        }

        private void ValidateStudentOnModify(Student student)
        {
            ValidateStudentIsNotNull(student);
        }

        public void ValidateStudentId(Guid studentId) =>
            Validate((Rule: IsInvalid(studentId), Parameter: nameof(Student.Id)));

        private static void ValidateStorageStudent(Student maybeStudent, Guid studentId)
        {
            if (maybeStudent is null)
            {
                throw new NotFoundStudentException(studentId);
            }
        }

        private static void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException(message: "Student is null.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };
        
        private static dynamic IsNotSame(
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
           };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidStudentException = 
                new InvalidStudentException(
                    message: "Invalid student. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidStudentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidStudentException.ThrowIfContainsErrors();
        }
    }
}