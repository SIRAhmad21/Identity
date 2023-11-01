using System.ComponentModel.DataAnnotations;

namespace TestIdentity.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Please Enter Your Email")]
        [EmailAddress(ErrorMessage ="Example user@hotmail.com")]
        [MaxLength(40,ErrorMessage ="Max Char 40 Latter")]
        public string? Email { get; set; }
        [Required(ErrorMessage ="Enter Your Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm Your Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Miss Match")]
        public string? ConfirmPassword { get; set; }

        public string? Mobile { get; set; }


    }
}
