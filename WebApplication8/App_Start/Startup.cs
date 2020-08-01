using AppBLL.Interfaces;
using AppBLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(WebApplication8.App_Start.Startup))]

namespace WebApplication8.App_Start
{
    public class Startup
    {
        readonly IServiceCreator serviceCreator = new ServiceCreator();

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.CreatePerOwinContext<IPostService>(CreatePostService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService();
        }

        private IPostService CreatePostService()
        {
            return serviceCreator.CreatePostService();
        }
    }    
}