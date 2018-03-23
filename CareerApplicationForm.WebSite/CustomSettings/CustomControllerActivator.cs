using CareerApplicationForm.WebSite.Controllers.Api;
using CareerApplicationForm.WebSite.Controllers.Plugins;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace CareerApplicationForm.WebSite.CustomSettings
{
    //this calss is to activate Simple injector for a specific controllers (i wanted to extract Umbraco Controllers)
    public class CustomControllerActivator : IHttpControllerActivator
    {
        private readonly Container container;
        private readonly IHttpControllerActivator original;

        public CustomControllerActivator(Container container, IHttpControllerActivator original)
        {
            this.container = container;
            this.original = original;
        }

        public IHttpController Create(
            HttpRequestMessage req, HttpControllerDescriptor desc, Type type)
        {
            if (type == typeof(JobApiController) || type == typeof(CandidateApiController))
                return (IHttpController)this.container.GetInstance(type);

            return this.original.Create(req, desc, type);
        }
    }
}