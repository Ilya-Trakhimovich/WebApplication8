using DataAcess.Entities;
using DataAcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repositories
{
    public class GroupPostRepository : IGroupPostRepository<GroupPost>
    {
        public EF.AppContext Db { get; set; }
        public GroupPostRepository(EF.AppContext db)
        {
            Db = db;
        }

        public void Add(GroupPost entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int groupPostId)
        {
            var groupPost = Db
                            .GroupPosts
                            .Where(x => x.Id == groupPostId)
                            .FirstOrDefault();
            
            Db.GroupPosts.Remove(groupPost);
            Db.SaveChanges();       
        }

        public IEnumerable<GroupPost> GetAll(int groupId)
        {
            throw new NotImplementedException();
        }

        public GroupPost GetById(int groupPostId)
        {
            throw new NotImplementedException();
        }
    }
}
