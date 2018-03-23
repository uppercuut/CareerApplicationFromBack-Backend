using CareerApplicationForm.Core.IServices;
using System.Web.Http;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace CareerApplicationForm.WebSite.Controllers.Api
{
    public class JobApiController : UmbracoApiController
    {
        private IAvailableJobService _IJobService;

        public JobApiController(IAvailableJobService _IJobService)
        {
            this._IJobService = _IJobService;
        }

        [HttpGet]
        public IHttpActionResult GetAllJobs()
        {
            
            //alias of availableJob is Web.config
            string availableJobAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.availableJob"];

            //implementation of the method can be found in AvailableJobService.cs 
            return Ok(_IJobService.GetAll(availableJobAlias));
        }



    }
}
