using System.ComponentModel.DataAnnotations;

namespace UserRoleTest.Models
{
    public class User
    {
        [Key]
        public int Id {get; set;}

        [Required(ErrorMessage = "Please fill in the Name")]
        public string Name {get; set;}

        [Required(ErrorMessage = "Please fill in the the Age")]
        [Range(1, int.MaxValue, ErrorMessage = "Age should be a positive number")]
        public int Age {get; set;}

        [Required(ErrorMessage = "Please fill in the the Email")]
        public string Email {get; set;}

        public List<Role> Roles {get; set;} = new();
    }
}
