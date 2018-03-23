using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CareerApplicationForm.Core.DomainModels
{
    public class JobCandidate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public HttpPostedFile Image { get; set; }
        [Required]
        public HttpPostedFile CV { get; set; }
        [Required]
        public int AppliedToJobID { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
