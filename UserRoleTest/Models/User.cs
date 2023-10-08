using System.ComponentModel.DataAnnotations;

namespace UserRoleTest.Models
{
    public class User
    {
        [Key]
        public int Id {get; set;}
        public string Name {get; set;}
        public int Age {get; set;}
        public string Email {get; set;}

        public List<Role> Roles {get; set;} = new();
    }
}
