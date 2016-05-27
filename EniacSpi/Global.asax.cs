using Dropbox.Api;
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


            Application["DropboxClient"] = new DropboxClient("gxr9u-7PkpAAAAAAAAAABwaRmUJXdxdqWgXiZ5x1CRp_qEC_9cb4p29xW69O6Gyb");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
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
