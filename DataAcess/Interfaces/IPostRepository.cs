using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Interfaces
{
    public interface IPostRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string id);
        T GetById(int id);
        void Add(T entity);
        void PostLike(PostLike entity);
        void PostDislike(PostLike entity);
        void Delete(int id);
    }
}
