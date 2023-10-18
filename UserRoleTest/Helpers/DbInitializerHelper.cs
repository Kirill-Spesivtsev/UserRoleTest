using UserRoleTest.Data;
using UserRoleTest.Models;

namespace UserRoleTest.Helpers
{
    public class DbInitializerHelper
    {
        public static async Task Seed(IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Users.Any() && !context.Roles.Any())
                {
                    context.Database.EnsureCreated();
                
                    var role1 = new Role{ Name = "User"};
                    var role2 = new Role{ Name = "Admin"};
                    var role3 = new Role{ Name = "Support"};
                    var role4 = new Role{ Name = "SuperAdmin"};

                    var users = new List<User>
                    {
                        new(){Name = "Salvador Sharpe", Age = 36, Email = "consectetuer@aol.net", Roles = new List<Role>{role1}},
                        new(){Name = "Dorothy Griffith", Age = 66, Email = "non.ante@yahoo.net", Roles = new List<Role>{role2}}, 
                        new(){Name = "Liberty Tate", Age = 51, Email = "egestas.ligula.nullam@outlook.org", Roles = new List<Role>{role3}},
                        new(){Name = "Craig Horton", Age = 61, Email = "sed.hendrerit.a@yahoo.ca", Roles = new List<Role>{role4}},
                        new(){Name = "Greg Carter", Age = 24, Email = "gravida@yahoo.org"},
                        new(){Name = "Karyn Gutierrez", Age = 29, Email = "nascetur.ridiculus@icloud.net"},
                        new(){Name = "Hu Bentley", Age = 20, Email = "arcu.eu@outlook.edu"},
                        new(){Name = "Jack Weaver", Age = 45, Email = "vitae.sodales.nisi@yahoo.net"},
                        new(){Name = "George Harvey", Age = 37, Email = "vehicula.risus.nulla@hotmail.couk"},
                        new(){Name = "Briar Villarreal", Age = 27, Email = "ac.orci.ut@yahoo.edu"}
                    };

                    context.Roles.AddRange(role1, role2, role3, role4);
                    context.Users.AddRange(users);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
