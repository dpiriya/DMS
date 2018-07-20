using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace DMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                // If caching roles in userData field then extract
                string[] userdata = authTicket.UserData.Split(new char[] { '|' });

                // Create the IIdentity instance
                IIdentity id = new FormsIdentity(authTicket);

                // Create the IPrinciple instance
                IPrincipal principal = new GenericPrincipal(id, userdata);

                // Set the context user
                Context.User = principal;

                //FormsAuthentication.SetAuthCookie(authCookie.Name, true);
            }
        }
    }
}
