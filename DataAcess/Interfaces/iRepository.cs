using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string id);
        T GetById(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Add(T entity);
        void PostLike(PostLike entity);
        void PostDislike(PostLike entity);
        void Update(T entity);
        void Delete(int id);
    }
}
