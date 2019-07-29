using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace OwinAuthenticationDemo
{
    //Creating custom authorize attribute is not really needed and [Authorize] should work just fine.
    //In scenarios where your application only supports Forms Authentication or keeps on redirecting to login 
    //below logic can be used to bypass and let Oauth server authenticate the user
    public class OwinAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext context)
        {
            if (HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "User is not authenticated" };
            else
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }
}