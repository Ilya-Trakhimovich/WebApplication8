using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Interfaces
{
    public interface IGroupPostRepository<T> where T : class
    {
        IEnumerable<T> GetAll(int groupId);
        T GetById(int postId);
        void Add(T entity);        
        void Delete(int groupPostId);

        //void PostLike(PostLike entity);
        //void PostDislike(PostLike entity);
    }
}
