using AppCore.DataAccess.Repositories;
using ETradeBusiness.Models;
using ETradeBusiness.Services;
using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;
using Microsoft.Owin.Security.OAuth;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ETradeWebApi.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // CORS: Cross Origin Resource Sharing
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new [] { "*" }); // *: tümü

            // Veritabanından kullanıcı kontrolü
            ETradeContext db = new ETradeContext();
            Repository<User> userRepository = new Repository<User>(db);
            Repository<Role> roleRepository = new Repository<Role>(db);
            UserService userService = new UserService(userRepository, roleRepository);
            UserModel userModel = userService.GetQuery().SingleOrDefault(user => user.UserName == context.UserName && user.Password == context.Password);
            if (userModel != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, userModel.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, userModel.Role.Name));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "User name or password is incorrect.");
            }
        }
    }
}