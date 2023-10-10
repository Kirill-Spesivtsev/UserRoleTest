using System.ComponentModel.DataAnnotations;

namespace UserRoleTest.Models
{
    public class Role
    {

        [Key]
        public int Id {get; set;}

        [Required(ErrorMessage = "Please fill in the Role Name")]
        public string Name {get; set;}

        public List<User> Users {get; set;} = new();

    }
}
