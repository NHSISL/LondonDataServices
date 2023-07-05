// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Configurations
{
    public class InvalidConfigurationException : Xeption
    {
        public InvalidConfigurationException(string message = "")
            : base(message: $"Invalid configuration. Please correct the errors and try again.  {message}")
        { }
    }
}