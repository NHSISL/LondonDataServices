// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs
{
    public class AddressToUprnFileLog : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int EntryCount { get; set; }
        public int EntriesMatched { get; set; }
        public int EntriesUnmatched { get; set; }
        public int ErrorRowCount { get; set; }
        public DateTimeOffset DateReceived { get; set; }
        public DateTimeOffset DateProcessed { get; set; }
        public TimeSpan Duration { get; set; }
        public SuccessStatus SuccessStatus { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
