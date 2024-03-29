﻿using API_QLXE.TimerControl;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace API_QLXE
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected async void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<API_QLXE.Models.QLXEContext>(null);
            CheckExpireOTPCodeTimer checkExpireOTPCode = new CheckExpireOTPCodeTimer();
            await checkExpireOTPCode.Start();
        }
    }
}
