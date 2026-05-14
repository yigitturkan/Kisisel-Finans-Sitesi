using System;
using System.Web;

namespace Web.Models
{
    public class EditProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        // Dosya yükleme işlemleri için HttpPostedFileBase kullanacağız
        public HttpPostedFileBase ProfileImage { get; set; }
    }
}