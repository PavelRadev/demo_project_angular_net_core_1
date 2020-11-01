using System.ComponentModel.DataAnnotations;

namespace API.Utils.Models
{
    public class PasswordChangeRequestModel
    {
        [Required]
        public string OldPassword { get; set; }
        
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}