// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Core.Models.Foundations.Documents
{
    public class Document
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] DocumentData { get; set; } = new byte[0];
    }
}
