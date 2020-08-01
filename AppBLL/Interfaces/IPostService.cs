using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Interfaces
{
    public interface IPostService : IDisposable
    {
        void AddPost(PostDTO postDTO); // create post
        void AddGroupPost(GroupPostDTO post);
        IEnumerable<PostDTO> GetAllPosts(string id);
        void DeletePost(int id);
        void DeleteGroupPost(int groupPostId);
        PostDTO GetPostById(int id);
    }
}
