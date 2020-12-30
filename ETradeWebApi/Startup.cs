using ETradeWebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(ETradeWebApi.Startup))]
namespace ETradeWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            ConfigureOAuth(appBuilder);

            WebApiConfig.Register(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
        }

        private void ConfigureOAuth(IAppBuilder appBuilder)
        {
            OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new Microsoft.Owin.PathString("/token"),
                //AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                AllowInsecureHttp = true,
                Provider = new SimpleAuthorizationServerProvider()
            };
            appBuilder.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}