// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs
{
    public class AddressToUprnFileLog
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
        public DateTimeOffset CreatedWhen { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedWhen { get; set; }
    }
}
