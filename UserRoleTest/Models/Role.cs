namespace UserRoleTest.Models
{
    public class Role
    {
        public Guid Id {get; set;}

        public string Name {get; set;}

        public List<Role> Users {get; set;} = new();

    }
}
