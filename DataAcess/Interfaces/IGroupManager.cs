using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Interfaces
{
    public interface IGroupManager<T> where T : class
    {
        void Create(T group);
        Group GetGroupById(int id);
        List<T> GetAllGroups();
        void SaveChanges();
        void UpdateGroupInformation(Group group);
        List<Group> GetGroupsByName(string groupName);
    }
}
