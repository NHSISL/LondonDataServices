// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
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

                string DocumentFileName = await optOutClient
                    .RetrieveOptOutStatusAsync(optOutFile: fileBytes, fileName: fileName);

                DocumentFileName.Should().NotBeNullOrWhiteSpace();

                Document retreiveDocument =
                    await documentService
                    .RetrieveDocumentByFileNameAsync(DocumentFileName);

                retreiveDocument.DocumentData.Should().NotBeNull();

                await documentService
                    .RemoveDocumentByFileNameAsync(DocumentFileName);
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }
        }
    }
}