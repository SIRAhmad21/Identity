using System.ComponentModel.DataAnnotations;

namespace TestIdentity.Models.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string? RoleId { get; set; }
        [Required(ErrorMessage ="Enter Role Name")]
        [MaxLength(25,ErrorMessage ="Max 25 ")]
        [MinLength(3,ErrorMessage ="Min Lentgh 3")]
        public string? RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
