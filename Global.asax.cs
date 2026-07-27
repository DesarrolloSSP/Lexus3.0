using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Lexus2._0.Clases; 



namespace Lexus2._0
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception rawEx = Server.GetLastError();
            Exception ex = rawEx.GetBaseException();
            Server.ClearError();
            ErrorL.Registrar(ex);   /// PROCESO para registrar los errores 
            if (ex is HttpException httpEx)
            {
                int statusCode = httpEx.GetHttpCode();
                switch (statusCode)
                {
                    case 403:
                        Response.Redirect("~/Error/Error403.aspx", true);
                        Context.ApplicationInstance.CompleteRequest();
                        return; 
                    case 404:
                        Response.Redirect("~/Error/Error404.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return; 
                }
            }
            Response.Redirect("~/Error/Error500.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }

}