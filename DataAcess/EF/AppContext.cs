using DataAcess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.EF
{
    public class AppContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        // IdentityDbContext already has Users and Roles properties that allow it to interact with user and role tables

        public AppContext(string conectionString) : base(conectionString) { }

        //public static AppContext Create()
        //{
        //    return new AppContext();
        //}

        static AppContext()
        {
            Database.SetInitializer<AppContext>(new AppUserInitializer());
        }

        //public AppContext(string connectionString) : base(connectionString) { }
    }
}
