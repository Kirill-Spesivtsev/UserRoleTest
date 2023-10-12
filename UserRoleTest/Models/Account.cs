using System.ComponentModel.DataAnnotations;

namespace UserRoleTest.Models
{
    public class Account
    {
        [Key]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
