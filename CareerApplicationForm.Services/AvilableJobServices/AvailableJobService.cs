using CareerApplicationForm.Core.DomainModels;
using CareerApplicationForm.Core.IServices;
using System;
using System.Collections.Generic;
using Umbraco.Core.Services;
using Umbraco.Web;
using System.Linq;

namespace CareerApplicationForm.Services.AvilableJobServices
{
    public class AvailableJobService : IAvailableJobService
    {
        private UmbracoHelper _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        public IEnumerable<AvailableJob> GetAll(string documentTypeAlias)
        {
            // Get all Jobs from the Umbraco tree 
            var jobs = _umbracoHelper.TypedContentAtXPath("//"+ documentTypeAlias);
            // Map the found nodes from IPublishedContent to a strongly typed object of type Job (defined below)
            var mappedJobs = jobs.Select(x => new AvailableJob
            {
                JobID = x.Id,                              
                JobName = x.Name,                      
            });
            return (mappedJobs);
        }

       

        //ToDo :
        public bool InsertToUmbracoContent(IContentService contentService, AvailableJob t, int parentId)
        {
            throw new NotImplementedException("Devloper:This method is not needed in the current business Context");
        }
    }
}
