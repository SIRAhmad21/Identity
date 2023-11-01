
using System.ComponentModel.DataAnnotations;

namespace TestIdentity.Models.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string? RoleName{ get; set; }
    }
}
