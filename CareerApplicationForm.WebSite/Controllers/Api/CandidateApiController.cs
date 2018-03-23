using CareerApplicationForm.Core.DomainModels;
using CareerApplicationForm.Core.IServices;
using CareerApplicationForm.Services.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace CareerApplicationForm.WebSite.Controllers.Api
{
    public class CandidateApiController : UmbracoApiController 
    {
        private readonly int CandidatenodeID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Content.ParentcandidatesCandidatenodeIDINT"]);

        private readonly string  HttpName = System.Configuration.ConfigurationManager.AppSettings["Http.Form.Name"]; 
             private readonly string  HttpEmail =System.Configuration.ConfigurationManager.AppSettings["Http.Form.Email"];  
             private readonly string  HttpAppliedToJobID = System.Configuration.ConfigurationManager.AppSettings["Http.Form.AppliedToJobID"];  
             private readonly string  HttpPhoneNumber = System.Configuration.ConfigurationManager.AppSettings["Http.Form.PhoneNumber"];
             private readonly string  HttpCV = System.Configuration.ConfigurationManager.AppSettings["Http.Form.CV"];
             private readonly string  HttpImage = System.Configuration.ConfigurationManager.AppSettings["Http.Form.Image"];

        private IJobCandidateService _ICandidateService;

        public CandidateApiController(IJobCandidateService _ICandidateService)
        {
            this._ICandidateService = _ICandidateService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Submit()
        {
            var httpRequest = System.Web.HttpContext.Current.Request;
            
            //making the From dynamic if changed from the font end from names are saved in the webconfig
            JobCandidate candidateModel = new JobCandidate()
            {
                Name = (httpRequest.Form[HttpName]!=null ? httpRequest.Form[HttpName].ToString() : ""),
                Email =(httpRequest.Form[HttpEmail] != null ? httpRequest.Form[HttpEmail].ToString() : ""),
                AppliedToJobID =(httpRequest.Form[HttpAppliedToJobID] != null ?int.Parse(httpRequest.Form[HttpAppliedToJobID]):0),
                PhoneNumber = (httpRequest.Form[HttpPhoneNumber] != null ? (httpRequest.Form[HttpPhoneNumber]) : ""),
                CV = (httpRequest.Files[HttpCV] != null ? (httpRequest.Files[HttpCV]) : null),
                Image = (httpRequest.Files[HttpImage] != null ? (httpRequest.Files[HttpImage]) : null),
            };

            //checking if the user sumbmitted a job to the same job role before.
            if (!_ICandidateService.IsVaildUser(candidateModel.Email, candidateModel.AppliedToJobID))
                return BadRequest("You Already submitted a job");

              var contentService = Services.ContentService;
            //after insertion of the user into umbraco  content tree we send the user an email. 
            if ( _ICandidateService.InsertToUmbracoContent(contentService, candidateModel, CandidatenodeID))
            {
                var Tolist = new List<string>() { candidateModel.Email };
                MailingService.SendMail("Dopravo careers", Tolist, "","" , "<p>Thank you for Submitting your application.</p>" +
                    "<p>we will be in touch soon.</p>",null);


            }
            return Ok(new { Error_Code = 0, Error_Desc = "sucess" });
        }




    }
}