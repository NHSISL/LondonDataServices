// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Ontologies;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ShouldRetrieveTerminologyArtifactsByOntologyAsset()
        {
            // given

            // Get Random Ontology Asset
            OntologyAsset randomOntologyAsset = CreateRandomOntologyAssets();
            OntologyAsset inputOntologyAsset = randomOntologyAsset;

            //TerminologyArtifact randomTerminologyArtifact =


            // Map Ontology Asset to Terminology Artifact
            // Check if terminology map exists, if exists update all props from full to last updated and mark is downloaded false.
            // If doesn’t exist store terminology artifact and ensure isCore and is downloaded = false

            // when

            // then
        }
    }
}
