using CareerApplicationForm.Core.DomainModels;
using CareerApplicationForm.Core.IServices;
using CareerApplicationForm.Services.Pocos;
using CareerApplicationForm.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace CareerApplicationForm.Services.JobCandidateService
{
    public class JobCandidateService : IJobCandidateService
    {
        private readonly int imageWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Images.WidthINT"]);    
        private readonly int parentImagesFolderID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Media.ParentImagesFolderIDINT"]);
        private readonly int parentFileFolderID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Media.ParentFilesFolderIDINT"]);
        private readonly string nodeAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.candidates"];
        private readonly string imageAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.candidates.picture"];
        private readonly string cvAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.candidates.cv"];
        private readonly string fullNameAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.candidates.fullName"];
        private readonly string jobAppliedToAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.candidates.jobAppliedTo"];
        private readonly string emailAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.candidates.email"];
        private readonly string phoneNumberAlias = System.Configuration.ConfigurationManager.AppSettings["UmbracoDocumentTypeAliases.candidates.phoneNumber"];
        private readonly UmbracoHelper umbracoHelper = new  UmbracoHelper(UmbracoContext.Current);

   
     

        public bool InsertToUmbracoContent(IContentService contentService, JobCandidate t, int parentId)
        {
            //create the node (the one new want to add)
            var newNode = contentService.CreateContent(t.Name, parentId, nodeAlias);

            //get job role the user submitted for 
            var node = contentService.GetById(t.AppliedToJobID );
            //get the Udi if tha job role node to be displiayed in the candidate record as content picker.
            var locaUdi = Udi.Create(Constants.UdiEntityType.Document, node.Key);

            //water mark the candidate image with the email declared at the webconfig
            Image image = ImageHelpers.AddWaterMark(t.Image);

            //resizing the images "ImageWidth" is set to 500 in the web.config
            image = ImageHelpers.FixedSize(image, imageWidth, image.Height);

            //save the new watermarked image in a physical location 
            var Locaction = ImageHelpers.SaveImage(image);
            //convert the physical location to visrtual to that can be read as byte
            string originalPath = ImageHelpers.ConvertFromPhysicalPath(Locaction);
            //convert to byte
            byte[] buffer = System.IO.File.ReadAllBytes(System.IO.Path.GetFullPath(HttpContext.Current.Server.MapPath(originalPath)));
            //but the byte in momery stream so that umbraco can handel it 
            System.IO.MemoryStream strm = new MemoryStream(buffer);

            IMediaService mediaService = ApplicationContext.Current.Services.MediaService;

            //************** Saving the image ********************
            var newImage = mediaService.CreateMedia(t.Name, parentImagesFolderID, "Image");
            newImage.SetValue("umbracoFile", t.Name + "." + System.IO.Path.GetExtension(t.Image.FileName), strm);
            mediaService.Save(newImage);
            newNode.SetValue(imageAlias, newImage.GetUdi().ToString());
            //************** Saving the image ********************

            //************** end Saving the file ********************
            var newCV = mediaService.CreateMedia(t.CV.FileName, parentFileFolderID, "File");
            newCV.SetValue("umbracoFile", t.CV);
            mediaService.Save(newCV);
            newNode.SetValue(cvAlias, newCV.GetUdi().ToString());
            //************** end Saving the image ********************

            //************** Saving user info ********************
            newNode.SetValue(fullNameAlias, t.Name);
            newNode.SetValue(jobAppliedToAlias, locaUdi.ToString());
            newNode.SetValue(emailAlias, t.Email);
            newNode.SetValue(phoneNumberAlias,t.PhoneNumber);
            //************** END Saving user info ********************

            //saving the pubshing into umbraco content 
            contentService.SaveAndPublishWithStatus(newNode);

            //delete the marked images from the file system
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(originalPath)))
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(originalPath));
            }
            ImageHelpers.DeleteTempImage(Locaction);
            return true;

        }

       
        public bool IsVaildUser(string Email ,int JobId)
        {
           
            var jobs = umbracoHelper.TypedContentAtXPath("//" + nodeAlias);
           
                //check if the user submitted the application is a first timer in this job role
                var User = jobs.SingleOrDefault(x => x.GetPropertyValue<string>(emailAlias).Trim().ToLower().Equals(Email.Trim().ToLower()) && x.GetPropertyValue<IPublishedContent>(jobAppliedToAlias).Id == JobId);
                if (User != null)
                {
                    return false;
                }
        
            

            return true;
        }

        public IEnumerable<JobCandidatesStringInformation> GetCandidateInfoAsstring(string BaseUrl)
        {
            var jobs = umbracoHelper.TypedContentAtXPath("//" + nodeAlias);

            //mapping the candidate
            var mappedJobs = jobs.Select(x => new JobCandidatesStringInformation
            {
                Name = x.GetPropertyValue<string>(fullNameAlias), 
                AppliedToJobID = umbracoHelper.TypedContent(x.GetPropertyValue<string>(jobAppliedToAlias)).Name,
                CV = BaseUrl+ umbracoHelper.TypedMedia(x.GetPropertyValue<string>(cvAlias)).Url,
                Email = x.GetPropertyValue<string>(emailAlias),
                Image = BaseUrl+ umbracoHelper.TypedMedia(x.GetPropertyValue<string>(imageAlias)).Url,
                PhoneNumber = x.GetPropertyValue<string>(phoneNumberAlias),
                
            });
            return (mappedJobs);
        }
    }
}
