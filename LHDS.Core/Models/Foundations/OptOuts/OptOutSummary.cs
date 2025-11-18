// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using CsvHelper.Configuration.Attributes;

namespace LHDS.Core.Models.Foundations.OptOuts
{
    public class OptOutSummary
    {
        [Name("Id")]
        public Guid Id { get; set; }
        [Name("NHSNo")]
        public string NhsNumber { get; set; } 
    }
}
