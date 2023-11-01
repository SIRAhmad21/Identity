using System.ComponentModel.DataAnnotations;

namespace TestIdentity.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter Your Email")]
        [EmailAddress(ErrorMessage = "Example user@hotmail.com")]
        [MaxLength(40, ErrorMessage = "Max Char 40 Latter")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; } 
        public bool RememberMe { get; set; } 

    }
}
