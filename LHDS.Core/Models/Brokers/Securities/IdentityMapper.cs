// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace LHDS.Core.Models.Brokers.Securities
{
    internal class IdentityMapper
    {
        public static EntraUser ToEntraUser(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) return null;


            var userIdString = claimsPrincipal.FindFirst("oid")?.Value
                ?? claimsPrincipal
                    .FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value

                ?? claimsPrincipal
                    .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value

                ?? claimsPrincipal
                    .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            var userId = userIdString;
            var givenName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value;
            var surname = claimsPrincipal.FindFirst(ClaimTypes.Surname)?.Value;
            var displayName = claimsPrincipal.FindFirst("displayName")?.Value;
            var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            var jobTitle = claimsPrincipal.FindFirst("jobTitle")?.Value;
            var roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList();
            var claimsList = claimsPrincipal.Claims;

            return new EntraUser(
                entraUserId: userId,
                givenName: givenName,
                surname: surname,
                displayName: displayName,
                email: email,
                jobTitle: jobTitle,
                roles: roles,
                claims: claimsList,
                authenticationType: claimsPrincipal.Identity.AuthenticationType);
        }

        public static ClaimsPrincipal ToClaimsPrincipal(EntraUser entraUser)
        {
            if (entraUser is null)
            {
                throw new ArgumentNullException(nameof(entraUser));
            }

            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(entraUser.EntraUserId))
            {
                claims.Add(new Claim("oid", entraUser.EntraUserId));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, entraUser.EntraUserId));
            }

            if (!string.IsNullOrWhiteSpace(entraUser.GivenName))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, entraUser.GivenName));
            }

            if (!string.IsNullOrWhiteSpace(entraUser.Surname))
            {
                claims.Add(new Claim(ClaimTypes.Surname, entraUser.Surname));
            }

            if (!string.IsNullOrWhiteSpace(entraUser.DisplayName))
            {
                claims.Add(new Claim("displayName", entraUser.DisplayName));
                claims.Add(new Claim(ClaimTypes.Name, entraUser.DisplayName));
            }

            if (!string.IsNullOrWhiteSpace(entraUser.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, entraUser.Email));
            }

            if (!string.IsNullOrWhiteSpace(entraUser.JobTitle))
            {
                claims.Add(new Claim("jobTitle", entraUser.JobTitle));
            }

            if (entraUser.Roles is not null)
            {
                foreach (var role in entraUser.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            if (entraUser.Claims is not null)
            {
                foreach (var c in entraUser.Claims)
                {
                    if (!claims.Any(existing => existing.Type == c.Type && existing.Value == c.Value))
                    {
                        claims.Add(c);
                    }
                }
            }

            var identity = new ClaimsIdentity(claims, authenticationType: "Custom");
            return new ClaimsPrincipal(identity);
        }
    }
}
