using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using OwinAuthenticationDemo.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OwinAuthenticationDemo.Provider;
using System.Web.Http;

//Tells OWIN to use the Startup class as the entry point for configuration
[assembly: OwinStartup(typeof(OwinAuthenticationDemo.Startup))]
namespace OwinAuthenticationDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            this.ConfigureOAuth(app);

            //Register Api endpoints (Remove from Global.asax.cs)
            WebApiConfig.Register(config);

            //Set ASPNET WebApi to run on top of OWIN.
            //OWIN/Katana(implementation of Owin specification) will send HTTP requests through ASPNET WebApi
            //Resides in package MS.ASPNET.WebApi.Owin
            app.UseWebApi(config);
            //app.UseServiceApiMembershipTokenAuthorization();
        }

        private void ConfigureOAuth (IAppBuilder app)
        {
            //app.CreatePerOwinContext<OwinAuthDbContext>
            //    (() => new OwinAuthDbContext());
            //app.CreatePerOwinContext<UserManager<IdentityUser>>(CreateManager);

            //Specify the token end point, token age, and if http is supported (maybe for Dev)
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new OAuthAppProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(3),
                AllowInsecureHttp = true,
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        //Microsoft.AspNet.Identity namespace has the class UserManager that exposes the 
        //use related to the API that automatically saves the changes to the UserStore.
        private static UserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, 
            IOwinContext context)
        {
            var userStore = new UserStore<IdentityUser>(context.Get<OwinAuthDbContext>());
            var owinManager = new UserManager<IdentityUser>(userStore);
            return owinManager;
        }

    }

}