// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.DataTypes;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class SubscriberAgreementControllerTests : RESTFulController
    {
        private readonly Mock<ISubscriberAgreementService> subscriberAgreementsServiceMock;
        private readonly SubscriberAgreementsController subscriberAgreementsController;

        public SubscriberAgreementControllerTests()
        {
            this.subscriberAgreementsServiceMock = new Mock<ISubscriberAgreementService>();
            this.subscriberAgreementsController = new SubscriberAgreementsController(this.subscriberAgreementsServiceMock.Object);
        }
    }
}
