﻿using System.Security.Claims;
using System.Threading.Tasks;
using AngularJS.Web.Security.Models;
using AngularJS.Web.Security.Repository;
using Microsoft.Owin.Security.OAuth;

namespace AngularJS.Web.Security.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            // context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            int id = 0;

            using (AuthRepository _repo = new AuthRepository())
            {
                ApplicationUser user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                id = user.Id;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("id", id.ToString()));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);

        }
    }
}