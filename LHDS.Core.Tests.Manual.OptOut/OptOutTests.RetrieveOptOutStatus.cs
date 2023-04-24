// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Manual.OptOut
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldRetreiveOptOutStatusAsync()
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(@"Resources\testfile.csv");
                FileInfo fi = new FileInfo(@"Resources\testfile.csv");
                var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
                await optOutClient.RetrieveOptOutStatusAsync(optOutFile: fileBytes, fileName: fileName);
                var outputLocation = $"{optOutConfiguration.OutputFolder}/{fileName}_Response_%DateStamp%.csv";
                output.WriteLine($"Check if new file is available at {outputLocation}");
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}