using CareerApplicationForm.Services.JobCandidateService;
using CareerApplicationForm.Services.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CareerApplicationForm.WebSite.Controllers.Plugins
{
    public class SendMailController : Umbraco.Web.Mvc.SurfaceController
    {
        [ValidateInput(false)]
        public ActionResult SendMail(string subject, List<string> To,string CC, string BCC, string body)
        {
            //sending mail to all Candidates
            JobCandidateService x = new JobCandidateService();
            var httpRequest = System.Web.HttpContext.Current.Request;
            HttpPostedFile  file = httpRequest.Files["file"];         
            MailingService.SendMail(subject, x.GetCandidateInfoAsstring("").Select(y=>y.Email).ToList<string>(),CC,BCC,body,file);


            return Json(new { Error_Code = 0, Error_Desc = "sucess" });
        }


    }
}