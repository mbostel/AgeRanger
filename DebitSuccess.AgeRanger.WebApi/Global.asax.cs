using DebitSuccess.AgeRanger.WebApi.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace DebitSuccess.AgeRanger.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            new ContainerBootstrap().Initialise();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
