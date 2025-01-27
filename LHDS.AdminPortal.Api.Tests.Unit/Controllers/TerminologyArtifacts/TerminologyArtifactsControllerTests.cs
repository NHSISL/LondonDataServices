// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.DataTypes;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class TerminologyArtifacts : RESTFulController
    {
        private readonly Mock<ITerminologyArtifactService> terminologyArtifactsServiceMock;
        private readonly TerminologyArtifactsController terminologyArtifactsController;

        public TerminologyArtifacts()
        {
            this.terminologyArtifactsServiceMock = new Mock<ITerminologyArtifactService>();
            this.terminologyArtifactsController = new TerminologyArtifactsController(this.terminologyArtifactsServiceMock.Object);
        }
    }
}
