using DataAcess.Entities;
using DataAcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repositories
{
    public class UserProfileManager : IUserProfileManager
    {
        public EF.AppContext Db { get; set; }

        public UserProfileManager(EF.AppContext db)
        {
            Db = db;
        }

        public void Create(UserProfile userProfile)
        {
            Db.UserProfiles.Add(userProfile);
            Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
