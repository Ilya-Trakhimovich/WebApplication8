using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Interfaces
{
    public interface IUserProfileManager : IDisposable
    {
        void Create(UserProfile userProfile);
        UserProfile GetUserById(string id);
        List<UserProfile> GetAllUsers();   
        void SaveChanges();
    }
}
