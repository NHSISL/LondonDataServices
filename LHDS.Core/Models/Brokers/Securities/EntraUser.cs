// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Security.Claims;

namespace LHDS.Core.Models.Brokers.Securities
{
    public class EntraUser
    {
        public EntraUser(
            string entraUserId,
            string givenName,
            string surname,
            string displayName,
            string email,
            string jobTitle,
            IEnumerable<string> roles,
            IEnumerable<Claim> claims,
            string authenticationType)
        {
            EntraUserId = entraUserId;
            GivenName = givenName;
            Surname = surname;
            DisplayName = displayName;
            Email = email;
            JobTitle = jobTitle;
            Roles = roles;
            Claims = claims;
        }

        public string EntraUserId { get; private set; } = string.Empty;
        public string GivenName { get; private set; } = string.Empty;
        public string Surname { get; private set; } = string.Empty;
        public string DisplayName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string JobTitle { get; private set; } = string.Empty;
        public string AuthenticationType { get; private set; } = string.Empty;
        public IEnumerable<string> Roles { get; private set; }
        public IEnumerable<Claim> Claims { get; private set; }
    }
}
