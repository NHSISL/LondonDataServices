// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Models.Foundations.SubscriberPractices
{
    public class SubscriberPractice : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid SubscriberAgreementId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PracticeCode { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public SubscriberAgreement SubscriberAgreement { get; set; }
    }
}
