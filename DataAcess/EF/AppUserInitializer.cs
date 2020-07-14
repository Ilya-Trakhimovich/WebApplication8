using DataAcess.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.EF
{
    public class AppUserInitializer : DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var role1 = new IdentityRole() { Name = "admin" };
            var role2 = new IdentityRole() { Name = "user" };

            roleManager.Create(role1);
            roleManager.Create(role2);

            db.SaveChanges();
        }
    }
}
