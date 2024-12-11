// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        [ReleaseCandidateFact(Skip = "Needs to be fixed.")]
        public async Task ShouldRetreiveOptOutStatusAsync()
        {
            // GIVEN
            byte[] fileBytes =
                File.ReadAllBytes(@"Resources\EmisNDOOExtract_2D2DB402-CD53-4523-9D84-BDC23A562C3D_20230516T144214.csv");

            FileInfo fi =
                new FileInfo(@"Resources\EmisNDOOExtract_2D2DB402-CD53-4523-9D84-BDC23A562C3D_20230516T144214.csv");

            var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            //// WHEN
            //string DocumentFileName = await optOutClient
            //    .RetrieveOptOutStatusAsync(input: fileBytes, fileName: fileName);

            // THEN
            //DocumentFileName.Should().NotBeNullOrWhiteSpace();

            //Document retreiveDocument =
            //    await documentService
            //    .RetrieveDocumentByFileNameAsync(DocumentFileName, encryptedFileContainer);

            //retreiveDocument.DocumentData.Should().NotBeNull();

            //await documentService
            //    .RemoveDocumentByFileNameAsync(DocumentFileName, encryptedFileContainer);
        }
    }
}