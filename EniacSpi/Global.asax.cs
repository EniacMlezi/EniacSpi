using Dropbox.Api;
using EniacSpi.Objects;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EniacSpi
{
    public static class DropboxEvents
    {
        private static event Action DropboxChangedEvent;

        public static void DropboxChanged()
        {
            DropboxChangedEvent?.Invoke();
        }
        public static void OnDropboxChanged(Action action)
        {
            DropboxChangedEvent += action;
        }
    }


    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            Application["DropboxClient"] = new DropboxClient("Zh6DFjG7MfAAAAAAAAAAB3_CItz3te1qllneUWKv46ItOMtmgmFYnGdYA4chJScr");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HostFinder.StartListening();
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateHeaderAntiForgeryToken : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
        }
    }
}
