using CareerApplicationForm.Core.DomainModels;


namespace CareerApplicationForm.Core.IServices
{
     public interface IJobCandidateService : IBaseServices<JobCandidate>
    {
      bool IsVaildUser(string Email, int JobId);

    }
}
