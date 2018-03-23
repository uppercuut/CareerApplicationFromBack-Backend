using System.Collections.Generic;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace CareerApplicationForm.Core.IServices
{
    public interface IBaseServices <T>
    {
        
        bool InsertToUmbracoContent(IContentService contentService, T t, int parentId);

    }
}
