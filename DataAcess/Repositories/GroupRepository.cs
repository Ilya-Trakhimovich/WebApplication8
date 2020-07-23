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
    public class GroupRepository : IGroupManager<Group>
    {
        public EF.AppContext Db { get; set; }

        public GroupRepository(EF.AppContext db)
        {
            Db = db;
        }
        public void Create(Group entity)
        {
            Db.Groups.Add(entity);
            Db.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<Group> GetAllGroups()
        {
            var groups = Db.Groups.ToList();
            return groups;
        }

        public Group GetGroupById(int id)
        {
            return Db.Groups.FirstOrDefault(x => x.Id == id);
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateGroupInformation(Group group)
        {
            var groupDB = Db.Groups.Where(x => x.Id == group.Id).FirstOrDefault();
            groupDB.GroupName = group.GroupName;
            groupDB.GroupDescription = group.GroupDescription;

            if (group.Avatar != null)
                groupDB.Avatar = group.Avatar;
            Db.SaveChanges();
        }

        public List<Group> GetGroupsByName(string groupName)
        {
            var groups = Db.Groups.Where(x => x.GroupName.Contains(groupName)).ToList();
            return groups;
        }       
    }
}
