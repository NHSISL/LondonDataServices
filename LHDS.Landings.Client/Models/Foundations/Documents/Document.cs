// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;

namespace LHDS.Landings.Client.Models.Foundations.Documents
{
    public class Document
    {
        public string FileName { get; set; }
        public Stream DocumentStream { get; set; }
    }
}
