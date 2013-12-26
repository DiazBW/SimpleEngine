using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace MvcApp.App_Start
{

    [assembly: OwinStartup(typeof(MvcApp.Startup))]
    public partial class Startup
    {
        static Startup()
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}