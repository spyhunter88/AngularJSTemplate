using System;
using System.Threading;
using System.Web.Http;
using AngularJS.Web.Security.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace AngularJS.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            ConfigureOAuth(app);

            //HttpConfiguration config = new HttpConfiguration();
            //WebApiConfig.Register(config);
            //ODataConfig.Register(config);
            //app.UseWebApi(config);

            // GlobalConfiguration.Configure(ODataConfig.Register);

            // We host WebAPI in Owin so we must Start manual or set OwinStartup
            // UnityWebApiActivator.Start(config);
            // UnityWebActivator.Start();

            //var context = new OwinContext(app.Properties);
            //var token = context.Get<CancellationToken>("host.OnAppDisposing");
            //if (token != CancellationToken.None)
            //{
            //    token.Register(() =>
            //    {
            //        // code to run
            //        UnityWebApiActivator.Shutdown();
            //        Console.WriteLine("Yeah, I am shutting down!");
            //    });
            //}
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}