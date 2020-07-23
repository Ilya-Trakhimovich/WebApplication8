using AutoMapper;
using DataAcess.Entities;
using DataAcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public void SaveChanges()
        {
            Db.SaveChanges();

        }

        public List<UserProfile> GetAllUsers()
        {
           var allUsers = Db.Users.ToList();

            List<UserProfile> userProfile = new List<UserProfile>();

            foreach (var user in allUsers)
            {
                userProfile.Add(new UserProfile()
                {
                    SecondName = user.UserProfile.SecondName,
                    FirstName = user.UserProfile.FirstName,
                    AboutMe = user.UserProfile.AboutMe,
                    Address = user.UserProfile.Address,
                    Avatar = user.UserProfile.Avatar,
                    Age = user.UserProfile.Age,
                    Education = user.UserProfile.Education,
                    Gender = user.UserProfile.Gender,
                    Id = user.UserProfile.Id,
                    ApplicationUser = user.UserProfile.ApplicationUser
                    
                });
            }
            return userProfile;
        }

        public UserProfile GetUserById(string id)
        {
            return Db.UserProfiles.FirstOrDefault(x => x.Id == id);
        }

    
        public void Dispose()
        {
           
            Db.Dispose();
        }

    }
}
