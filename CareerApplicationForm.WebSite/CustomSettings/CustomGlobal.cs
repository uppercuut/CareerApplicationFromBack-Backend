using CareerApplicationForm.Core.IServices;
using CareerApplicationForm.Services.AvilableJobServices;
using CareerApplicationForm.Services.JobCandidateService;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Umbraco.Core;

namespace CareerApplicationForm.WebSite.CustomSettings
{
    public class CustomGlobal : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //Register json formating into the webSite GlobalConfiguration
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //Register the interface to with the controllers 
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Register<IAvailableJobService, AvailableJobService>(Lifestyle.Transient);
            container.Register<IJobCandidateService, JobCandidateService>(Lifestyle.Transient);


            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
    new CustomControllerActivator(
        container,
        GlobalConfiguration.Configuration.Services.GetHttpControllerActivator()));
        }
    }
    }
