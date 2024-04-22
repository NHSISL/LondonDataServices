// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Tynamix.ObjectFiller;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Extensions
{
    public partial class ExceptionExtensionTests
    {
        private readonly ITestOutputHelper output;

        public ExceptionExtensionTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
