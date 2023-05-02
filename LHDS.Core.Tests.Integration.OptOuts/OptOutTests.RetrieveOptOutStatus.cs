// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        [Fact(Skip = "Excluded from pipeline")]
        public async Task ShouldRetreiveOptOutStatusAsync()
        {
            //TODO:  fix from DH to replace this tests

            //try
            //{
            //    byte[] fileBytes = File.ReadAllBytes(@"Resources\testfile.csv");
            //    FileInfo fi = new FileInfo(@"Resources\testfile.csv");
            //    var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            //    string DocumentFileName = await optOutClient
            //        .RetrieveOptOutStatusAsync(optOutFile: fileBytes, fileName: fileName);

            //    DocumentFileName.Should().NotBeNullOrWhiteSpace();

            //    Document retreiveDocument =
            //        await documentService
            //        .RetrieveDocumentByFileNameAsync(DocumentFileName);

            //    retreiveDocument.DocumentData.Should().NotBeNull();

            //    await documentService
            //        .RemoveDocumentByFileNameAsync(DocumentFileName);
            //}
            //catch (Exception ex)
            //{
            //    Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            //}
        }
    }
}