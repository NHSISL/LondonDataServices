// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldRetreiveOptOutStatusAsync()
        {
            // GIVEN
            string encryptedFileContainer = "emislanding";

            byte[] fileBytes =
                File.ReadAllBytes(@"Resources\EmisNDOOExtract_2D2DB402-CD53-4523-9D84-BDC23A562C3D_20230516T144214.csv");

            Stream fileStream = new MemoryStream(fileBytes);
            Stream expectedFileStream = new MemoryStream();

            FileInfo fi =
                new FileInfo(@"Resources\EmisNDOOExtract_2D2DB402-CD53-4523-9D84-BDC23A562C3D_20230516T144214.csv");

            var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            //// WHEN
            string documentFileName = await optOutClient
                .RetrieveOptOutStatusAsync(input: fileStream, fileName: fileName);

            // THEN
            documentFileName.Should().NotBeNullOrWhiteSpace();

            await documentService
                    .RetrieveDocumentByFileNameAsync(expectedFileStream, documentFileName, encryptedFileContainer);

            expectedFileStream.Should().NotBeNull();

            await documentService
                .RemoveDocumentByFileNameAsync(documentFileName, encryptedFileContainer);

            await Task.CompletedTask;
        }
    }
}