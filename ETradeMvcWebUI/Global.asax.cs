using ETradeDataAccess.Configs;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace ETradeMvcWebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ETradeConfig.ConnectionString = ConfigurationManager.ConnectionStrings["ETradeContext"].ConnectionString;
        }
    }
}
