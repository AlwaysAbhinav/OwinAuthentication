using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OwinAuthenticationDemo.Provider
{
    public class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        //Validates the "client_id". Call the validate method of Owin OAuth when passed the username and password
        //Alternatively the credentails can be extracted from HTTP authenticate header through context.TryGetBasicCredentials(out clientId, out clientSecret)
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string clientId;
            //string clientSecret;

            context.Validated();

            //if (context.TryGetBasicCredentials(out clientId, out clientSecret))
            //{
            //    context.Validated();
            //}
            //else
            //{
            //    context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization Header");
            //    context.Rejected();
            //}
        }

        //Authenticates the credentials through GetUserAsync method that is created under Repository class
        //If user is found then validate the request else set an error
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //UserManager<IdentityUser> userManager = context.OwinContext.GetUserManager<UserManager<IdentityUser>>();
            //IdentityUser user;
            //try
            //{
            //    user = await userManager.FindAsync(context.UserName, context.Password);
            //}
            //catch
            //{
            //    context.SetError("server_error");
            //    context.Rejected();
            //    return;
            //}
            //if (user != null)
            //{
            //    ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
            //    context.Validated(identity);
            //}
            //else
            //{
            //    context.SetError("invalid_grant", "Invalid User Id or Password");
            //    context.Rejected();
            //}

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (OwinApiAuthRepository authRepository = new OwinApiAuthRepository())
            {
                IdentityUser user = await authRepository.GetUserAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "Invalid UserName or Password");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //After an identity is created there are alerady three or more claims, User ID, user name, identity provider for each role assigned
            //Add any additional claims you want, they will be set during the login process
            //Claims can contain Roles and more and can be used too implement Role-Based-Access control )RBAC
            //This is usually persisted in an authentication cookie so that there isn't a hit to the DB for every HTTP request
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            
            //e.g. add other custom claim types
            //identity.AddClaim(new Claim("FullName", user.UserName));
            //identity.AddClaim(new Claim("role", "user"));

            //If you would like to get the claim information from an identity
            //var prinicpal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //var email = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            context.Validated(identity);

        }

    }
}