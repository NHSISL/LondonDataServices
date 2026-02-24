// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LHDS.Core.Services.Foundations.FileNameValidations;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationServiceTests
    {
        private readonly IFileNameValidationService fileNameValidationService;

        public FileNameValidationServiceTests()
        {
            this.fileNameValidationService = new FileNameValidationService();
        }

        public static TheoryData<Exception> DependencyValidationExceptions()
        {
            return new TheoryData<Exception>()
            {
                GetRegexParseException(),
                GetRegexMatchTimeoutException(),
                new ArgumentNullException()
            };
        }

        private static RegexParseException GetRegexParseException() =>
            (RegexParseException)RuntimeHelpers.GetUninitializedObject(typeof(RegexParseException));

        private static RegexMatchTimeoutException GetRegexMatchTimeoutException() =>
            (RegexMatchTimeoutException)RuntimeHelpers.GetUninitializedObject(typeof(RegexMatchTimeoutException));

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}