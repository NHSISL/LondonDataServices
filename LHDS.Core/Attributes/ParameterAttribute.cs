// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Attributes
{
    /// <summary>
    /// Indicate to the serializer that this property or value 
    /// has a different representation when being serialized to JSON
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ParameterAttribute : Attribute
    {
        /// <summary>
        /// The key to use in place of the property's name
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// The name to use in place of the enum's value
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}