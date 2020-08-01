using DataAcess.Entities;
using DataAcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repositories
{
    public class PostRepository : IPostRepository<Post>
    {
        public EF.AppContext Db { get; set; }

        public PostRepository(EF.AppContext db)
        {
            Db = db;
        }

        public Post GetById(int id)
        {
          return  Db.Posts
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public void Add(Post entity)
        {
            Db.Posts.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(int id)
        {
            var post = Db.Posts
                .Where(x => x.Id == id)
                .FirstOrDefault();

            Db.Posts.Remove(post);
            Db.SaveChanges();
        }

        IEnumerable<Post> IPostRepository<Post>.GetAll(string id)
        {
            return Db.Posts
                .Where(x => x.UserPageId == id)
                .ToList();
        }

        public void PostLike(PostLike post)
        {
            Db.Posts.Where(x => x.Id == post.PostId)
                .FirstOrDefault()
                .PostLikes.Add(post);

            Db.SaveChanges();
        }

        public void PostDislike(PostLike post)
        {
            var postLikeToDelete = Db.PostLikes
                .Where(x => x.PostId == post.PostId && x.User == post.User)
                .FirstOrDefault();

            Db.PostLikes.Remove(postLikeToDelete);
            Db.SaveChanges();
        }
    }
}
