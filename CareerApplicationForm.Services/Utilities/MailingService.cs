using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CareerApplicationForm.Services.Utilities
{
  public  class MailingService
    {


        static private readonly string _UserName = (System.Configuration.ConfigurationManager.AppSettings["Mailingservice.Sender.UserName"]);
        static private readonly string _Password = (System.Configuration.ConfigurationManager.AppSettings["Mailingservice.Sender.Password"]);
        static private readonly string _SmtpServer = (System.Configuration.ConfigurationManager.AppSettings["Mailingservice.SmtpServer"]);
        static private readonly int Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Mailingservice.SmtpServer.Port"]);

        public static void SendMail(string subject,List<string> TO , string CC, string BCC, string body,HttpPostedFile file)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(_SmtpServer);
            mailMessage.From = new MailAddress(_UserName);
            FillTo(ref mailMessage, TO);
            FillCc(ref mailMessage, CC);
            FillBcc(ref mailMessage, BCC);
       
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                var attachment = new Attachment(file.InputStream, fileName);
                mailMessage.Attachments.Add(attachment);
            }
            SmtpServer.Port = Port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(_UserName, _Password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mailMessage);

        }
        static private void FillTo(ref MailMessage mailMessage , List<string> To)
        {
            foreach (var address in To.Distinct())
            {
                mailMessage.To.Add(address);
            }
            
        }
        static  private void FillCc(ref MailMessage mailMessage ,string CC)
        {
            if (!CC.Contains(",")&& !string.IsNullOrEmpty(CC))
            {
                mailMessage.CC.Add(CC);
                return;
            }
            foreach (var address in CC.Split(',').ToList<string>().Distinct())
                if (!string.IsNullOrEmpty(address))
                    mailMessage.CC.Add(address);
        }
        static private void FillBcc(ref MailMessage mailMessage,string Bcc)
        {
            if (!Bcc.Contains(",")&&!string.IsNullOrEmpty(Bcc))
            {
                mailMessage.Bcc.Add(Bcc);
                return;
            }

            foreach (var address in Bcc.Split(',').ToList<string>().Distinct())
                if(!string.IsNullOrEmpty(address))
                mailMessage.Bcc.Add(address);
        }
    }
}
