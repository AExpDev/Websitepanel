﻿using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebsitePanel.WebDav.Core.Interfaces.Security;
using WebsitePanel.WebDav.Core.Security;
using WebsitePanel.WebDav.Core.Security.Authentication;
using WebsitePanel.WebDav.Core.Security.Cryptography;
using WebsitePanel.WebDavPortal.DependencyInjection.Providers;
using WebsitePanel.WebDavPortal.Models;

namespace WebsitePanel.WebDavPortal.DependencyInjection
{
    public class PortalDependencies
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind<HttpSessionState>().ToProvider<HttpSessionStateProvider>();
            kernel.Bind<ICryptography>().To<CryptoUtils>();
            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>();
            kernel.Bind<IWebDavManager>().ToProvider<WebDavManagerProvider>();
        }
    }
}