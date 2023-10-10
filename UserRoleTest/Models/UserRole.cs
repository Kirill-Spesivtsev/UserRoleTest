using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserRoleTest.Models
{
    public class UserRole
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int RoleId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        public Role Role { get; set; } = null!;
    }
}
