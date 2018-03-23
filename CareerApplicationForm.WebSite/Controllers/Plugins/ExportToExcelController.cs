using CareerApplicationForm.Services.JobCandidateService;
using CareerApplicationForm.Services.Pocos;
using CareerApplicationForm.Services.Utilities;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Web.Mvc;


namespace CareerApplicationForm.WebSite.Controllers.Plugins
{
    
    public class ExportToExcelController : Umbraco.Web.Mvc.SurfaceController
    {
      

            // GET: ExportToExcel
         [HttpPost]
        public ActionResult Export()
        {
            var jobCandidateService =new  JobCandidateService();
           
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(ExportingHelpers.ConvertToDataTable<JobCandidatesStringInformation>(jobCandidateService.GetCandidateInfoAsstring(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/")));

            using (XLWorkbook workBook = new XLWorkbook())
            {
                workBook.Worksheets.Add(dataSet);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename="+DateTime.Now.ToShortDateString()+".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    workBook.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return Json(new { Error_Code = 0, Error_Desc = "sucess" });
        }

    
      
    }
}