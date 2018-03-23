using System.Net.Http.Headers;
using System.Web.Http;

namespace CareerApplicationForm.WebSite.CustomSettings
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Formating Response as json Only some the defualt for some brwsers is xml 
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}