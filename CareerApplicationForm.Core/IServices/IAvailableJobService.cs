using CareerApplicationForm.Core.DomainModels;
using System.Collections.Generic;

namespace CareerApplicationForm.Core.IServices
{
   public interface IAvailableJobService : IBaseServices<AvailableJob>
    {
        IEnumerable<AvailableJob> GetAll(string documentTypeAlias);
    }
}
